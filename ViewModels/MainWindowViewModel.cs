namespace UniHacker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Description => Language.GetString(nameof(Description));
        public string SelectTips => Language.GetString(nameof(SelectTips));
        public string Select => Language.GetString(nameof(Select));
        public string Version => Language.GetString(nameof(Version));
        public string Status => Language.GetString(nameof(Status));
        public string Patch_btn => Language.GetString(nameof(Patch_btn));
        public string Revert_btn => Language.GetString(nameof(Revert_btn));
    }
}
