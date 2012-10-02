using System;
using System.IO;
using System.Reflection;
using System.Collections;

namespace RipLeech {
   public class Sounds : System.IDisposable {
      #region Private Members
      private  ArrayList   _sounds     = new ArrayList();
      #endregion

      #region ctors
      internal Sounds(Assembly target) {
         foreach(string resource in target.GetManifestResourceNames()) 
            if (Path.GetExtension(resource).ToLower() == ".wav") 
               _sounds.Add(new Sound(resource, target.GetManifestResourceStream(resource)));
      }
      #endregion

      #region Public Properties
      public Sound this[string name] {
         get {
            foreach(Sound se in _sounds)
               if (se.Name == name.ToLower())   // Search case-insensitive
                  return se;

            return null;
         }
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         foreach (Sound se in _sounds)
            se.Dispose();

         GC.SuppressFinalize(this);
      }
      #endregion
   }

   public class Sound : System.IDisposable {
      [System.Runtime.InteropServices.DllImport("Winmm.dll")]
      private static extern bool PlaySound(byte[] data, IntPtr hMod, UInt32 dwFlags);
      private const UInt32 SND_ASYNC   = 1;
      private const UInt32 SND_MEMORY  = 4;

      #region Private Members
      private     string   _name    = string.Empty;
      private     byte[]   _data    = null;
      #endregion

      #region ctors
      internal Sound(string name, System.IO.Stream stream) {
         Int32    length = (Int32)stream.Length;
         string[] tokens = name.Split('.');

         // Pluck the simple name of the resource out of
         // the fully qualified string.  tokens[tokens.Length - 1]
         // is the file extension, also not needed.
         _name = tokens[tokens.Length - 2];

         _data = new byte[length];
         stream.Read(_data, 0, length);
      }
      #endregion

      #region Public Properties
      public string Name {
         get {return _name;}
      }
      #endregion

      #region Public Methods
      public void Play() {
         PlaySound(_data, IntPtr.Zero, SND_ASYNC | SND_MEMORY);
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         GC.SuppressFinalize(this);
      }
      #endregion
   }
}