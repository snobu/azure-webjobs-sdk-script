// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Script.WebHost.Models
{
    public class AssignmentContext
    {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("appSettings")]
        public Dictionary<string, string> AppSettings { get; set; }
    }

    public static class AssignementContextExtensions
    {
        public static string GetZipUrl(this AssignmentContext context)
        {
            // TODO: move to const file
            const string setting = "WEBSITE_USE_ZIP";
            return context.AppSettings.ContainsKey(setting)
                ? context.AppSettings[setting]
                : string.Empty;
        }
    }
}