<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="false" Inherits="CKFinder.Settings.ConfigFile" %>
<%@ Import Namespace="CKFinder.Settings" %>
<script runat="server">
    /**
     * This function must check the user session to be sure that he/she is
     * authorized to upload and access files using CKFinder.
     */
    public override bool CheckAuthentication()
    {
        return true; // Always return true for this example
    }

    /**
     * All configuration settings must be defined here.
     */
    public override void SetConfig()
    {
        // License details
        LicenseName = "khoapham.vn";
        LicenseKey = "RVAYQC1GCSKUPE2EDXS9RS9GLGY93E8V";

        // Base URL and directory for storing files
        BaseUrl = "/DATA";
        BaseDir = HttpContext.Current.Server.MapPath("/DATA");

        // Optional: enable extra plugins
        Plugins = new string[] {
            // "CKFinder.Plugins.FileEditor, CKFinder_FileEditor",
            // "CKFinder.Plugins.ImageResize, CKFinder_ImageResize",
            // "CKFinder.Plugins.Watermark, CKFinder_Watermark"
        };

        // Plugin settings
        PluginSettings = new Hashtable
        {
            { "ImageResize_smallThumb", "90x90" },
            { "ImageResize_mediumThumb", "120x120" },
            { "ImageResize_largeThumb", "180x180" },
            { "Watermark_source", "logo.gif" },
            { "Watermark_marginRight", "5" },
            { "Watermark_marginBottom", "5" },
            { "Watermark_quality", "90" },
            { "Watermark_transparency", "80" }
        };

        // Image settings
        Images.MaxWidth = 1600;
        Images.MaxHeight = 1200;
        Images.Quality = 80;

        // Security and file checks
        CheckSizeAfterScaling = true;
        DisallowUnsafeCharacters = true;
        CheckDoubleExtension = true;
        ForceSingleExtension = true;
        HtmlExtensions = new string[] { "html", "htm", "xml", "js" };
        HideFolders = new string[] { ".*", "CVS" };
        HideFiles = new string[] { ".*" };
        SecureImageUploads = true;
        RoleSessionVar = "CKFinder_UserRole";

        // Access Control
        AccessControl acl = AccessControl.Add();
        acl.Role = "*";
        acl.ResourceType = "*";
        acl.Folder = "/";
        acl.FolderView = true;
        acl.FolderCreate = true;
        acl.FolderRename = true;
        acl.FolderDelete = true;
        acl.FileView = true;
        acl.FileUpload = true;
        acl.FileRename = true;
        acl.FileDelete = true;

        // Resource Types
        DefaultResourceTypes = "";

        ResourceType type;
        type = ResourceType.Add("Images");
        type.Url = BaseUrl + "/Images/";
        type.Dir = BaseDir == "" ? "" : BaseDir + "/Images/";
        type.MaxSize = 0;
        type.AllowedExtensions = new string[] { "bmp", "gif", "jpeg", "jpg", "png" };
        type.DeniedExtensions = new string[] { };

        ResourceType type2;
        type2 = ResourceType.Add("FILES");
        type2.Url = BaseUrl + "/FILES/";
        type2.Dir = BaseDir == "" ? "" : BaseDir + "/FILES/";
        type2.MaxSize = 0;
        type2.AllowedExtensions = new string[] { };
        type2.DeniedExtensions = new string[] { };
    }
</script>
