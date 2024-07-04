using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence
{
    public class NmsAssistantContext : DbContext
    {
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackQuestion> FeedbackQuestions { get; set; }
        public DbSet<FeedbackAnswer> FeedbackAnswers { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<Version> Versions { get; set; }
        public DbSet<GuideMeta> GuideMetaDatas { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<WhatIsNew> WhatIsNews { get; set; }
        public DbSet<CommunityLink> CommunityLinks { get; set; }
        public DbSet<PendingGuide> PendingGuides { get; set; }
        public DbSet<HelloGamesHistory> HelloGamesHistories { get; set; }
        public DbSet<LanguageSubmission> LanguageSubmissions { get; set; }
        public DbSet<GuideDetail> GuideDetails { get; set; }
        public DbSet<GuideMetaGuideDetail> GuideMetaGuideDetails { get; set; }
        public DbSet<FriendCode> FriendCodes { get; set; }
        public DbSet<PendingFriendCode> PendingFriendCodes { get; set; }
        public DbSet<OnlineMeetup2020Submission> OnlineMeetup2020Submissions { get; set; }
        public DbSet<WeekendMission> WeekendMissions { get; set; }
        public DbSet<CommunitySpotlight> CommunitySpotlights { get; set; }
        public DbSet<Expedition> Expeditions { get; set; }
        public DbSet<CommunityMissionRecord> CommunityMissionRecords { get; set; }
        public DbSet<CommunityMissionRecordTier> CommunityMissionRecordTiers { get; set; }
        public DbSet<CommunityMissionProgress> CommunityMissionsProgress { get; set; }
        public DbSet<MonitorRecord> MonitorRecords { get; set; }
        public DbSet<SteamUpdateEvent> UpdateEventSteams { get; set; }
        public DbSet<UpdateEvent> UpdateEventGenerics { get; set; }


        public NmsAssistantContext(DbContextOptions<NmsAssistantContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseSqlServer("data source=localhost;initial catalog=NMS_Assistant;Integrated Security=True;MultipleActiveResultSets=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Feedback.MapRelationships(modelBuilder);
            FeedbackQuestion.MapRelationships(modelBuilder);
            FeedbackAnswer.MapRelationships(modelBuilder);
            Donation.MapRelationships(modelBuilder);
            User.MapRelationships(modelBuilder);
            Permission.MapRelationships(modelBuilder);
            UserPermission.MapRelationships(modelBuilder);
            Contributor.MapRelationships(modelBuilder);
            Version.MapRelationships(modelBuilder);
            GuideMeta.MapRelationships(modelBuilder);
            Setting.MapRelationships(modelBuilder);
            Testimonial.MapRelationships(modelBuilder);
            WhatIsNew.MapRelationships(modelBuilder);
            CommunityLink.MapRelationships(modelBuilder);
            PendingGuide.MapRelationships(modelBuilder);
            HelloGamesHistory.MapRelationships(modelBuilder);
            LanguageSubmission.MapRelationships(modelBuilder);
            GuideDetail.MapRelationships(modelBuilder);
            GuideMetaGuideDetail.MapRelationships(modelBuilder);
            FriendCode.MapRelationships(modelBuilder);
            PendingFriendCode.MapRelationships(modelBuilder);
            OnlineMeetup2020Submission.MapRelationships(modelBuilder);
            WeekendMission.MapRelationships(modelBuilder);
            CommunitySpotlight.MapRelationships(modelBuilder);
            Expedition.MapRelationships(modelBuilder);
            CommunityMissionRecord.MapRelationships(modelBuilder);
            CommunityMissionRecordTier.MapRelationships(modelBuilder);
            CommunityMissionProgress.MapRelationships(modelBuilder);
            MonitorRecord.MapRelationships(modelBuilder);
            SteamUpdateEvent.MapRelationships(modelBuilder);
            UpdateEvent.MapRelationships(modelBuilder);
        }
    }

}
