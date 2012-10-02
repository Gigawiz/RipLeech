using System;
using System.Reflection;

namespace RipLeech {
   public class Resources : System.IDisposable {
      #region Private Members
      private  Bitmaps     _bitmaps = null;
      private  Icons       _icons   = null;
      private  Sounds      _sounds  = null;
      #endregion

      #region ctors
      public Resources(Assembly target) {
         _bitmaps = new Bitmaps(target);
         _icons   = new Icons(target);
         _sounds  = new Sounds(target);
      }
      #endregion

      #region Public Properies
      public Bitmaps Bitmaps {
         get {return _bitmaps;}
      }

      public Icons Icons {
         get {return _icons;}
      }

      public Sounds Sounds {
         get {return _sounds;}
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         _bitmaps.Dispose();
         _icons.Dispose();
         _sounds.Dispose();

         GC.SuppressFinalize(this);
      }
      #endregion
   }
}
