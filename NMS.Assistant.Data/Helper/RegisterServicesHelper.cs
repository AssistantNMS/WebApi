using BTS.App.Data.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using NMS.Assistant.Data.Cache;
using NMS.Assistant.Data.Cache.Interface;
using NMS.Assistant.Data.Repository;
using NMS.Assistant.Data.Service;
using NMS.Assistant.Data.Service.Interface;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Integration.Repository;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Integration.Service;
using NMS.Assistant.Integration.Service.Interface;
using NMS.Assistant.Persistence.Repository;
using NMS.Assistant.Persistence.Repository.Interface;
using System.Net.Http;

namespace NMS.Assistant.Data.Helper
{
    public static class RegisterServicesHelper
    {
        public static IServiceCollection RegisterCommonServices(this IServiceCollection services, IApiConfiguration config)
        {
            // Config Singletons
            services.AddSingleton(config);
            services.AddSingleton<IJwt>(construct => config.Jwt);
            services.AddSingleton<IRedis>(construct => config.Redis);
            services.AddSingleton<IStripe>(construct => config.Stripe);
            services.AddSingleton<IWebhook>(construct => config.Webhook);
            services.AddSingleton<ILogging>(construct => config.Logging);
            services.AddSingleton<ITwitter>(construct => config.Twitter);
            services.AddSingleton<ISendGrid>(construct => config.SendGrid);
            services.AddSingleton<IFirebase>(construct => config.Firebase);
            services.AddSingleton<IDatabase>(construct => config.Database);
            services.AddSingleton<IDirectory>(construct => config.Directory);
            services.AddSingleton<IAuthentication>(construct => config.Authentication);
            services.AddSingleton<ISharedDirectory>(construct => config.SharedDirectory);

            //https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            services.AddTransient<HttpClient>();

            // Repositories
            services.AddTransient<IJwtRepository, JwtRepository>();
            services.AddTransient<IDonationRepository, DonationRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IContributorRepository, ContributorRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IVersionRepository, VersionRepository>();
            services.AddTransient<IGuideMetaRepository, GuideMetaRepository>();
            services.AddTransient<ISettingRepository, SettingRepository>();
            services.AddTransient<ITestimonialRepository, TestimonialRepository>();
            services.AddTransient<IWhatIsNewRepository, WhatIsNewRepository>();
            services.AddTransient<ISlackRepository, SlackRepository>();
            services.AddTransient<ICommunityLinkRepository, CommunityLinkRepository>();
            services.AddTransient<IPendingGuideRepository, PendingGuideRepository>();
            services.AddTransient<IHelloGamesHistoryRepository, HelloGamesHistoryRepository>();
            services.AddTransient<ILanguageSubmissionRepository, LanguageSubmissionRepository>();
            services.AddTransient<IGuideDetailRepository, GuideDetailRepository>();
            services.AddTransient<IFriendCodeRepository, FriendCodeRepository>();
            services.AddTransient<IOnlineMeetup2020SubmissionRepository, OnlineMeetup2020SubmissionRepository>();
            services.AddTransient<IWeekendMissionRepository, WeekendMissionRepository>();
            services.AddTransient<ICommunitySpotlightRepository, CommunitySpotlightRepository>();
            services.AddTransient<IExpeditionRepository, ExpeditionRepository>();
            services.AddTransient<ICommunityMissionRepository, CommunityMissionRepository>();
            services.AddTransient<ICommunityMissionProgressRepository, CommunityMissionProgressRepository>();
            services.AddTransient<IMonitorRecordRepository, MonitorRecordRepository>();
            services.AddTransient<SteamUpdateEventRepository, SteamUpdateEventRepository>();
            services.AddTransient<UpdateEventRepository, UpdateEventRepository>();

            services.AddTransient<INoMansSkyWebScrapeRepository, NoMansSkyWebScrapeRepository>();
            services.AddTransient<IGalacticAtlasRepository, GalacticAtlasRepository>();
            services.AddTransient<IGithubRepository, GithubRepository>();
            services.AddTransient<ITwitterRepository, TwitterRepository>();
            services.AddTransient<IFirebaseRepository, FirebaseRepository>();
            services.AddTransient<IFaqBotRepository, FaqBotRepository>();
            services.AddTransient<IRedisRepository, RedisRepository>();
            services.AddTransient<IMonstaAppRepository, MonstaAppRepository>();
            services.AddTransient<IPurgoMalumRepository, PurgoMalumRepository>();
            services.AddTransient<ISendGridRepository, SendGridRepository>();
            services.AddTransient<IDownloadFileRepository, DownloadFileRepository>();
            services.AddTransient<INmsfmRepository, NmsfmRepository>();
            services.AddTransient<IUptimeRobotRepository, UptimeRobotRepository>();
            services.AddTransient<NmscdCommunitySearchRepository>();
            services.AddTransient<NmsSocialRepository>();
            services.AddTransient<NmsSocialAdminRepository>();

            // MemoryCache stuffs
            services.AddSingleton<ICustomCache, CustomCache>();
            
            // Services
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ISteamNewsService, SteamNewsService>();


            // Conditional

            //if (config.Database.Type.Equals(DatabaseType.MicrosoftSql))
            //{
            //    services.AddTransient<IZooqleDatabaseRepository, ZooqleDatabaseRepository>();
            //}

            //if (config.Database.Type.Equals(DatabaseType.SqlLite))
            //{
            //    services.AddTransient<IZooqleDatabaseRepository, ZooqleSqlLiteDatabaseRepository>();
            //}

            return services;
        }

    }
}
