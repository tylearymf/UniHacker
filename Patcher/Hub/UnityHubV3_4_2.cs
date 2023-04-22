using System;
using System.IO;
using System.Threading.Tasks;
#if !DOCKER_ENV
using MessageBoxAvalonia = MessageBox.Avalonia;
#endif

namespace UniHacker
{
    internal class UnityHubV3_4_2
    {
        const string getValidEntitlementGroups = @"
        return __awaiter(this, void 0, void 0, function* () {
            return [{
                startDate: new Date('1993-01-01T08:00:00.000Z'),
                expirationDate: new Date('9999-01-01T08:00:00.000Z'),
                productName: __importDefault(require(""../../i18nHelper"")).default.i18n.translate('license-management:TYPE_EDUCATION'),
                licenseType: 'ULF',
            }];
        });
";

        const string isLicenseValid = @"
        return __awaiter(this, void 0, void 0, function* () {
            return true;
        });
";

        const string licensingSdk_init = @"
        return __awaiter(this, void 0, void 0, function* () {
            return true;
        });
";
        const string licensingSdk_getInstance = @"
		return null;
 ";

        public static async Task<bool> Patch(string exportFolder)
        {
#if DOCKER_ENV
            Program.TryGetEnvironmentVariable(Program.NEED_LOGIN, out var needLogin);
            if (string.Compare(needLogin, bool.TrueString, true) == 0)
#else
            var result = await MessageBox.Show(Language.GetString("Hub_Patch_Option_Login"), MessageBoxAvalonia.Enums.ButtonEnum.YesNo);
            if (result == MessageBoxAvalonia.Enums.ButtonResult.No)
#endif
            {
                var defaultLocalConfigPath = Path.Combine(exportFolder, "build/common/DefaultLocalConfig.js");
                var defaultLocalConfigContent = File.ReadAllText(defaultLocalConfigPath);
                defaultLocalConfigContent = defaultLocalConfigContent.Replace("DisableSignIn]: false,", "DisableSignIn]: true,");
                File.WriteAllText(defaultLocalConfigPath, defaultLocalConfigContent);
            }

#if DOCKER_ENV
            Program.TryGetEnvironmentVariable(Program.DISABLE_UPDATE, out var disableUpdate);
            if (string.Compare(disableUpdate, bool.TrueString, true) == 0)
#else
            result = await MessageBox.Show(Language.GetString("Hub_Patch_Option_DisableUpdate"), MessageBoxAvalonia.Enums.ButtonEnum.YesNo);
            if (result == MessageBoxAvalonia.Enums.ButtonResult.Yes)
#endif
            {
                var defaultLocalConfigPath = Path.Combine(exportFolder, "build/common/DefaultLocalConfig.js");
                var defaultLocalConfigContent = File.ReadAllText(defaultLocalConfigPath);
                defaultLocalConfigContent = defaultLocalConfigContent.Replace("DisableAutoUpdate]: false,", "DisableAutoUpdate]: true,");
                File.WriteAllText(defaultLocalConfigPath, defaultLocalConfigContent);
            }

            var licenseServicePath = Path.Combine(exportFolder, "build/main/services/licenseService/licenseService.js");
            var licenseServiceContent = File.ReadAllText(licenseServicePath);
            UnityHubPatcher.ReplaceMethodBody(ref licenseServiceContent, @"isLicenseValid", isLicenseValid);
            File.WriteAllText(licenseServicePath, licenseServiceContent);

            var licenseServiceCorePath = Path.Combine(exportFolder, "build/main/services/licenseService/licenseServiceCore.js");
            var licenseServiceCoreContent = File.ReadAllText(licenseServiceCorePath);
            UnityHubPatcher.ReplaceMethodBody(ref licenseServiceCoreContent, @"static getValidEntitlementGroups", getValidEntitlementGroups);
            File.WriteAllText(licenseServiceCorePath, licenseServiceCoreContent);

            var licensingSdkPath = Path.Combine(exportFolder, "build/main/services/licenseService/licensingSdk.js");
            var licensingSdkContent = File.ReadAllText(licensingSdkPath);
            UnityHubPatcher.ReplaceMethodBody(ref licensingSdkContent, @"init", licensingSdk_init);
            UnityHubPatcher.ReplaceMethodBody(ref licensingSdkContent, @"getInstance", licensingSdk_getInstance);
            File.WriteAllText(licensingSdkPath, licensingSdkContent);

            var editorappPath = Path.Combine(exportFolder, "build/main/services/editorApp/editorapp.js");
            var editorappContent = File.ReadAllText(editorappPath);
            editorappContent = editorappContent.Replace("licensingSdk.getInstance().", "licensingSdk.getInstance()?.");
            File.WriteAllText(editorappPath, editorappContent);

            var editorManagerPath = Path.Combine(exportFolder, "build/main/services/editorManager/editorManager.js");
            var editorManagerContent = File.ReadAllText(editorManagerPath);
            editorManagerContent = editorManagerContent.Replace("return this.validateEditorFile(location, skipSignatureCheck)", "return this.validateEditorFile(location, true)");
            File.WriteAllText(editorManagerPath, editorManagerContent);

            return true;
        }
    }
}
