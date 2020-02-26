using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Cloud.InstrumentationFramework;

namespace TailSpin.SpaceGame.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //IfxInitializer.Initialize("Session");
            EmitMetrics();
            // THIS IS REQUIRED to get the role instance where your app is running 
            // You will need this IP address to initialize Ifx 
            // Reference: https://genevamondocs.azurewebsites.net/collect/references/antares/ifx.html 
            System.Net.IPAddress[] addresses = System.Net.Dns.GetHostAddresses(Environment.MachineName);
            string ipAddress = null;

            foreach (var addr in addresses)
            {
                if (addr.ToString() == "127.0.0.1")
                {
                    continue;
                }
                else if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = addr.ToString();
                    break;
                }
            }
           
            // IFx initialization is a required step for emitting logs
            // Initialize Ifx using the Tenant and Role names that you used in your configuration 
            // Use the IP address you got above as the Role instance 
            IfxInitializer.IfxInitialize("TTGTestTenant", "Template", ipAddress);
            //IfxInitializer.IfxInitialize("TestAutoIFxEvent");
            // IfxInitializer.IFxHeartBeatInterval.ToString();
            EmitLogs();
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        static void EmitMetrics()
        {
            ErrorContext mdmError = new ErrorContext();

            MeasureMetric1D testMeasure = MeasureMetric1D.Create(
                "TrustToolsAutoWPTest",
                "TrustToolsAutoWPTest",
                "TTAutoTestMetric",
                "TTAutoTestDimension",
                ref mdmError);

            if (testMeasure == null)
            {
                Console.WriteLine("Fail to create MeasureMetric, error code is {0:X}, error message is {1}",
                    mdmError.ErrorCode,
                    mdmError.ErrorMessage);
            }
            else if (!testMeasure.LogValue(101, "TTAutoTestDimension", ref mdmError))
            {
                Console.WriteLine("Fail to log MeasureMetric value, error code is {0:X}, error message is {1}",
                    mdmError.ErrorCode,
                    mdmError.ErrorMessage);
            }
        }

        /// <summary>
        /// Log an application audit event.
        /// </summary>
        /// <param name="auditMandatoryProperties">The mandatory properties to be logged.</param>
        /// <param name="auditOptionalProperties">The optional properties to be logged.</param>
        /// <returns>Whether the audit event was logged successfully.</returns>
        
        static void EmitLogs()
        {
            using (Operation operation = new Operation("TTGTestTenantOperation"))
            {
                string webAppName = System.Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");

                AuditMandatoryProperties auditMandatoryProperties = new AuditMandatoryProperties();
                auditMandatoryProperties.OperationName = "genevaTTTest";
                auditMandatoryProperties.ResultType = OperationResult.Timeout;
                auditMandatoryProperties.AddAuditCategory(AuditEventCategory.UserManagement);
                auditMandatoryProperties.AddCallerIdentity(new CallerIdentity(CallerIdentityType.Username, "genevaTTTest"));
                //auditMandatoryProperties.AddTargetResource("TestTarget", "TestTargetValue");
                auditMandatoryProperties.AddTargetResource("Web App Name", webAppName);

                // And the most important part, calling the Audit functions: 
                bool result = IfxAudit.LogApplicationAudit(auditMandatoryProperties, null) &&
                              IfxAudit.LogManagementAudit(auditMandatoryProperties, null);

                operation.SetResult(OperationResult.Success);
            }
        }
    }
}
