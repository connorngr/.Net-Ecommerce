namespace Innerglow_App.Models
{
    public class Summernote
    {
        public Summernote(string IdEditor, bool Loadlibrary1 = true) 
        { 
            IDEditor = IdEditor;
            LoadLibrary = Loadlibrary1;
        }
        public string IDEditor { get; set; }

        public bool LoadLibrary { get; set; }

        public int height { get; set; } = 120;

        public string toolbar { get; set; } = @"[
                ['style', ['style']],
                ['font', ['bold', 'underline', 'clear']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['table', ['table']],
                ['insert', ['link', 'picture', 'video']],
                ['view', ['fullscreen', 'codeview', 'help']]
            ]
        ";

    }
}


