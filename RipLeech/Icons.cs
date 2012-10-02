using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Collections;

namespace RipLeech {
   public class Icons : System.IDisposable {
      #region Private Members
      private  ArrayList      _icons   = new ArrayList();
      #endregion

      #region ctors
      internal Icons(Assembly target) {
         foreach(string resource in target.GetManifestResourceNames())
            if (Path.GetExtension(resource).ToLower() == ".ico")
               _icons.Add(new IconEx(resource, new Icon(target.GetManifestResourceStream(resource))));
      }
      #endregion

      #region Public Properies
      public Icon this[string name] {
         get {
            foreach(IconEx ie in _icons)
               if (ie.Name == name.ToLower())   // Search case-insensitive
                  return ie.Icon;

            return null;
         }
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         foreach(IconEx ie in _icons)
            ie.Dispose();

         GC.SuppressFinalize(this);
      }
      #endregion

      private class IconEx : System.IDisposable {
         #region Private Members
         private  string      _name    = string.Empty;
         private  Icon        _icon    = null;
         #endregion

         #region ctors
         public IconEx(string name, Icon icon) {
            string[] tokens = name.Split('.');

            // Pluck the simple name of the resource out of
            // the fully qualified string.  tokens[tokens.Length - 1]
            // is the file extension, also not needed.
            _name = tokens[tokens.Length - 2].ToLower();
            _icon = icon;
         }
         #endregion

         #region Public Properties
         public string Name {
            get {return _name;}
         }

         public Icon Icon {
            get {return _icon;}
         }
         #endregion

         #region IDisposable Members
         public void Dispose() {
            _icon.Dispose();
         }
         #endregion
      }
   }
}
