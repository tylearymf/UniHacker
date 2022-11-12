using System.IO;
using System.Threading.Tasks;
#if !DOCKER_ENV
using MessageBoxAvalonia = MessageBox.Avalonia;
#endif

namespace UniHacker
{
    internal class UnityHubV3
    {
        const string init = @"
        return __awaiter(this, void 0, void 0, function* () {
            this.authLogger.info('Initializing the auth service');
            this.initNetworkInterceptors();
            electron_1.powerMonitor.on('resume', () => {
                this.authLogger.debug('Resetting token watcher timeout after system resume');
                this.monitorTokens();
            });
            this.onNetworkUp();
            this.logInWithAccessToken();
            return this.userInfo;
        });
";
        const string openSignIn = @"
        this.logInWithAccessToken();
";
        const string logInWithAccessToken = @"
        return __awaiter(this, void 0, void 0, function* () {
            try {
                this.authLogger.info('Fetching user info from the identity provider using access token');
                this.userInfo = this.getFormattedUserInfo();
                this.emit(PostalTopic_1.default.USER_INFO_UPDATED, this.userInfo);
                this.setLoggedInFlags();
                this.monitorTokens();
                this.emit(AuthEvents_1.default.LOGGED_IN_WITH_ACCESS_TOKEN, undefined);
                return this.userInfo;
            }
            catch (error) {
                this.authLogger.error(`Error fetching user info from Access Token: ${error}`);
                HeapService_1.default.sendSignInErrorEvent('logInWithAccessToken');
                this.setLoggedOutFlags();
                throw error;
            }
        });
";
        const string isTokenValid = @"
        return true;
";

        const string getValidEntitlementGroups = @"
        return __awaiter(this, void 0, void 0, function* () {
            return [{
                startDate: new Date('1993-01-01T08:00:00.000Z'),
                expirationDate: new Date('9999-01-01T08:00:00.000Z'),
                productName: i18nHelper_1.default.i18n.translate('license-management:TYPE_PRO'),
                licenseType: 'PRO',
            }];
        });
";

        const string isLicenseValid = @"
        return __awaiter(this, void 0, void 0, function* () {
            return true;
        });
";

        const string fetchUserInfo = @"
		return {
			foreign_key: 'anonymous',
			name: 'anonymous',
			email: 'anonymous@gmail.com',
			primary_org: 'anonymous',
			identifier: 'anonymous',
			created_at: 0,
		}
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
#if !DOCKER_ENV
            var result = await MessageBox.Show(Language.GetString("Hub_Patch_Option_Login"), MessageBoxAvalonia.Enums.ButtonEnum.YesNo);
            if (result == MessageBoxAvalonia.Enums.ButtonResult.No)
#endif
            {
                var authServicePath = Path.Combine(exportFolder, "build/main/services/authService/AuthService.js");
                var authServiceContent = File.ReadAllText(authServicePath);
                UnityHubPatcher.ReplaceMethodBody(ref authServiceContent, @"init", init);
                UnityHubPatcher.ReplaceMethodBody(ref authServiceContent, @"openSignIn", openSignIn);
                UnityHubPatcher.ReplaceMethodBody(ref authServiceContent, @"logInWithAccessToken", logInWithAccessToken);
                UnityHubPatcher.ReplaceMethodBody(ref authServiceContent, @"static\sisTokenValid", isTokenValid);
                File.WriteAllText(authServicePath, authServiceContent);

                var cloudCorePath = Path.Combine(exportFolder, "build/main/services/cloudCore/cloudCore.js");
                var cloudCoreContent = File.ReadAllText(cloudCorePath);
                UnityHubPatcher.ReplaceMethodBody(ref cloudCoreContent, @"fetchUserInfo", fetchUserInfo);
                File.WriteAllText(cloudCorePath, cloudCoreContent);
            }

#if !DOCKER_ENV
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
            UnityHubPatcher.ReplaceMethodBody(ref licenseServiceContent, @"getValidEntitlementGroups", getValidEntitlementGroups);
            UnityHubPatcher.ReplaceMethodBody(ref licenseServiceContent, @"isLicenseValid", isLicenseValid);
            File.WriteAllText(licenseServicePath, licenseServiceContent);

            var licensingSdkPath = Path.Combine(exportFolder, "build/main/services/licenseService/licensingSdk.js");
            var licensingSdkContent = File.ReadAllText(licensingSdkPath);
            UnityHubPatcher.ReplaceMethodBody(ref licensingSdkContent, @"init", licensingSdk_init);
            UnityHubPatcher.ReplaceMethodBody(ref licensingSdkContent, @"getInstance", licensingSdk_getInstance);
            File.WriteAllText(licensingSdkPath, licensingSdkContent);

            var editorappPath = Path.Combine(exportFolder, "build/main/services/editorApp/editorapp.js");
            var editorappContent = File.ReadAllText(editorappPath);
            editorappContent = editorappContent.Replace("licensingSdk.getInstance().", "licensingSdk.getInstance()?.");
            File.WriteAllText(editorappPath, editorappContent);

            return true;
        }
    }
}
