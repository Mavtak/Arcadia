using System.Collections.Generic;

namespace SomewhatGeeky.Arcadia.Engine.Items
{
    public class FilenameFlag : GenericLibraryItem
    {
        private List<Language> impliedLanguages;
        private NumberRange impliedPlayers;
        private FileIntegrity impliedFileHealth = FileIntegrity.Unknown;

        #region constructors

        public FilenameFlag(string name, ArcadiaLibrary parentGameLibrary)
            : base(name, parentGameLibrary)
        { }
        public FilenameFlag(string name)
            : this(name, null)
        { }
        public FilenameFlag(ArcadiaLibrary parentGameLibrary)
            : this(null, parentGameLibrary)
        { }
        public FilenameFlag()
            : this(null, null)
        { }

        #endregion

    }
}
