//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Cloud.InstrumentationFramework;

//namespace Tailspin.SpaceGame.Web
//{
//    public class IfxTest
//    {
//        static void Main(string[] args)
//        {
//            EmitMetrics();
//            // THIS IS REQUIRED to get the role instance where your app is running 
//            // You will need this IP address to initialize Ifx 
//            // Reference: https://genevamondocs.azurewebsites.net/collect/references/antares/ifx.html 
//            System.Net.IPAddress[] addresses = System.Net.Dns.GetHostAddresses(Environment.MachineName);
//            string ipAddress = null;

//            foreach (var addr in addresses)
//            {
//                if (addr.ToString() == "127.0.0.1")
//                {
//                    continue;
//                }
//                else if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
//                {
//                    ipAddress = addr.ToString();
//                    break;
//                }
//            }
//            // IFx initialization is a required step for emitting logs
//            // Initialize Ifx using the Tenant and Role names that you used in your configuration 
//            // Use the IP address you got above as the Role instance 
//            IfxInitializer.IfxInitialize("TTGTestTenant", "Template", ipAddress);
//            //IfxInitializer.IfxInitialize("TestAutoIFxEvent");
//           // IfxInitializer.IFxHeartBeatInterval.ToString();
//            EmitLogs();
//        }

//        static void EmitMetrics()
//        {
//            ErrorContext mdmError = new ErrorContext();

//            MeasureMetric1D testMeasure = MeasureMetric1D.Create(
//                "TrustToolsAutoWPTest",
//                "TrustToolsAutoWPTest",
//                "TTAutoTestMetric",
//                "TTAutoTestDimension",
//                ref mdmError);

//            if (testMeasure == null)
//            {
//                Console.WriteLine("Fail to create MeasureMetric, error code is {0:X}, error message is {1}",
//                    mdmError.ErrorCode,
//                    mdmError.ErrorMessage);
//            }
//            else if (!testMeasure.LogValue(101, "TTAutoTestDimension", ref mdmError))
//            {
//                Console.WriteLine("Fail to log MeasureMetric value, error code is {0:X}, error message is {1}",
//                    mdmError.ErrorCode,
//                    mdmError.ErrorMessage);
//            }
//        }

//        static void EmitLogs()
//        {
//            using (Operation operation = new Operation("TTGTestTenantOperation"))
//            {
//                operation.SetResult(OperationResult.Success);
//            }
//        }
//    }
//}
