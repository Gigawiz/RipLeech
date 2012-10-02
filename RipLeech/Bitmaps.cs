using System;
using System.IO;
using System.Drawing;
using System.Collections;

namespace RipLeech {
   public class Bitmaps : System.IDisposable {
      #region Private Members
      private ArrayList    _bitmaps    = new ArrayList();
      #endregion

      #region ctors
      internal Bitmaps(System.Reflection.Assembly target) {
         foreach(string resource in target.GetManifestResourceNames()) {
            string ext = Path.GetExtension(resource).ToLower();

            if (ext == ".bmp" || 
                  ext == ".gif" ||
                  ext == ".jpg" ||
                  ext == ".jpeg") 
               _bitmaps.Add(new BitmapEx(resource, (Bitmap)Bitmap.FromStream(target.GetManifestResourceStream(resource))));
         }
      }
      #endregion

      #region Public Properties
      public Bitmap this[string name] {
         get {
            foreach (BitmapEx b in _bitmaps)
               if (b.Name == name.ToLower())   // Search case-insensitive
                  return b.Bitmap;

            return null;
         }
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         foreach(BitmapEx bmx in _bitmaps)
            bmx.Dispose();

         GC.SuppressFinalize(this);
      }
      #endregion

      private class BitmapEx : System.IDisposable {
         #region Private Members
         private  string      _name    = string.Empty;
         private  Bitmap      _bitmap  = null;
         #endregion

         #region ctors
         public BitmapEx(string name, Bitmap bitmap) {
            string[] tokens = name.Split('.');

            // Pluck the simple name of the resource out of
            // the fully qualified string.  tokens[tokens.Length - 1]
            // is the file extension, also not needed.
            _name = tokens[tokens.Length - 2].ToLower();
            _bitmap = bitmap;
         }
         #endregion

         #region Public Properties
         public string Name {
            get {return _name;}
         }

         public Bitmap Bitmap {
            get {return _bitmap;}
         }
         #endregion

         #region IDisposable Members
         public void Dispose() {
            _bitmap.Dispose();
            _bitmap = null;
         }
         #endregion
      }
	}
}