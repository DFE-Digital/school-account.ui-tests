﻿using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace DFE.SA.UITests.Settings
{
    public sealed class ConfigurationService
    {
        private static readonly IConfigurationRoot Root = InitializeConfiguration();

        public ConfigurationService()
        {
        }

        public static TSection GetSection<TSection>()
            where TSection : class, new()
        {
            string sectionName = MakeFirstLetterToLower(typeof(TSection).Name);

            return Root.GetSection(sectionName).Get<TSection>();
        }

        private static string MakeFirstLetterToLower(string text)
        {
            return char.ToLower(text[0]) + text.Substring(1);
        }

        public static WebSettings GetWebSettings()
        {
            WebSettings result = Root.GetSection(key: "websettings").Get<WebSettings>();
            if (result == null)
            {
                throw new ConfigurationNotFoundException(typeof(WebSettings).ToString());
            }
            return result;
        }

        private static IConfigurationRoot InitializeConfiguration()
        {
            var filesInExecutionDir = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var settingsFile = filesInExecutionDir.FirstOrDefault(x => x.Contains("appSettings") && x.EndsWith(".json"));
            var builder = new ConfigurationBuilder();
            if (settingsFile != null)
            {
                builder.AddJsonFile(settingsFile, optional: true, reloadOnChange: true);
            }

            return builder.Build();
        }
    }
}
