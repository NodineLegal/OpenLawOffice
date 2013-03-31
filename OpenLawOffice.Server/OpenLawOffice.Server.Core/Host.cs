using System;
using AutoMapper;

namespace OpenLawOffice.Server.Core
{
    public class Host : Singleton<Host>
    {
        public WebAppHost WebHost { get; set; }

        public Host()
        {
            Common.ObjectMapper.MapAssembly(typeof(Common.ObjectMapper).Assembly);
            Common.ObjectMapper.MapAssembly(typeof(Host).Assembly);
            WebHost = new WebAppHost();
            WebHost.Init();
        }

        public class WebAppHost : ServiceStack.WebHost.Endpoints.AppHostBase
        {
            // Commented lines are in place to support AddinFramework which will be a future feature
            //private static Dictionary<System.Type, string[]> _services = new Dictionary<System.Type, string[]>();

            public WebAppHost()
                : base("OpenLawOffice.Server", typeof(WebAppHost).Assembly)
            {
            }

            public static void LazyRegisterService<T>(string[] atRestPaths)
            {
                //_services.Add(typeof(T), atRestPaths);
            }

            public override void Configure(Funq.Container container)
            {
                //Dictionary<System.Type, string[]>.Enumerator en = _services.GetEnumerator();
                //while (en.MoveNext())
                //{
                //    RegisterService(en.Current.Key, en.Current.Value);
                //}
                //en.Dispose();


                base.SetConfig(new ServiceStack.WebHost.Endpoints.EndpointHostConfig
                {
                    GlobalResponseHeaders = {
                        { "Access-Control-Allow-Origin", "*" },
                        { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE" },
                        { "Access-Control-Allow-Headers", "Content-Type" },
                    },
                });

                new Installation.Database().Run();
            }

        }
    }
}
