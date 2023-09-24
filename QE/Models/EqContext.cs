using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QE.Models;

public partial class EqContext : DbContext
{
    public EqContext()
    {
    }

    public EqContext(DbContextOptions<EqContext> options)
        : base(options)
    {
    }

    public EqContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<AuthGroup> AuthGroups { get; set; }

    public virtual DbSet<AuthGroupPermission> AuthGroupPermissions { get; set; }

    public virtual DbSet<AuthPermission> AuthPermissions { get; set; }

    public virtual DbSet<DTicket> DTickets { get; set; }

    public virtual DbSet<DTicketArchve> DTicketArchves { get; set; }

    public virtual DbSet<DTicketPrerecord> DTicketPrerecords { get; set; }

    public virtual DbSet<DTicketStatus> DTicketStatuses { get; set; }

    public virtual DbSet<DTicketStatusArchve> DTicketStatusArchves { get; set; }

    public virtual DbSet<DjangoAdminLog> DjangoAdminLogs { get; set; }

    public virtual DbSet<DjangoContentType> DjangoContentTypes { get; set; }

    public virtual DbSet<DjangoMigration> DjangoMigrations { get; set; }

    public virtual DbSet<DjangoSession> DjangoSessions { get; set; }

    public virtual DbSet<SDayWeek> SDayWeeks { get; set; }

    public virtual DbSet<SEmployee> SEmployees { get; set; }

    public virtual DbSet<SOffice> SOffices { get; set; }

    public virtual DbSet<SOfficePrerecord> SOfficePrerecords { get; set; }

    public virtual DbSet<SOfficeSchedule> SOfficeSchedules { get; set; }

    public virtual DbSet<SOfficeScoreboard> SOfficeScoreboards { get; set; }

    public virtual DbSet<SOfficeScoreboardMultimedium> SOfficeScoreboardMultimedia { get; set; }

    public virtual DbSet<SOfficeScoreboardService> SOfficeScoreboardServices { get; set; }

    public virtual DbSet<SOfficeTerminal> SOfficeTerminals { get; set; }

    public virtual DbSet<SOfficeTerminalButton> SOfficeTerminalButtons { get; set; }

    public virtual DbSet<SOfficeTerminalService> SOfficeTerminalServices { get; set; }

    public virtual DbSet<SOfficeWindow> SOfficeWindows { get; set; }

    public virtual DbSet<SOfficeWindowService> SOfficeWindowServices { get; set; }

    public virtual DbSet<SPriority> SPriorities { get; set; }

    public virtual DbSet<SRole> SRoles { get; set; }

    public virtual DbSet<SService> SServices { get; set; }

    public virtual DbSet<SSourcePrerecord> SSourcePrerecords { get; set; }

    public virtual DbSet<SStatus> SStatuses { get; set; }

    public virtual DbSet<SWeekend> SWeekends { get; set; }

    public virtual DbSet<ThumbnailKvstore> ThumbnailKvstores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersGroup> UsersGroups { get; set; }

    public virtual DbSet<UsersUserPermission> UsersUserPermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=176.113.83.242;User Id=postgres;Password=!ShamiL19;Port=5432;Database=EQ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("auth_group_pkey");

            entity.ToTable("auth_group");

            entity.HasIndex(e => e.Name, "auth_group_name_a6ea08ec_like").HasOperators(new[] { "varchar_pattern_ops" });

            entity.HasIndex(e => e.Name, "auth_group_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<AuthGroupPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("auth_group_permissions_pkey");

            entity.ToTable("auth_group_permissions");

            entity.HasIndex(e => e.GroupId, "auth_group_permissions_group_id_b120cbf9");

            entity.HasIndex(e => new { e.GroupId, e.PermissionId }, "auth_group_permissions_group_id_permission_id_0cd325b0_uniq").IsUnique();

            entity.HasIndex(e => e.PermissionId, "auth_group_permissions_permission_id_84c5c92e");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");

            entity.HasOne(d => d.Group).WithMany(p => p.AuthGroupPermissions)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_group_permissions_group_id_b120cbf9_fk_auth_group_id");

            entity.HasOne(d => d.Permission).WithMany(p => p.AuthGroupPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_group_permissio_permission_id_84c5c92e_fk_auth_perm");
        });

        modelBuilder.Entity<AuthPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("auth_permission_pkey");

            entity.ToTable("auth_permission");

            entity.HasIndex(e => e.ContentTypeId, "auth_permission_content_type_id_2f476e4b");

            entity.HasIndex(e => new { e.ContentTypeId, e.Codename }, "auth_permission_content_type_id_codename_01ab375a_uniq").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codename)
                .HasMaxLength(100)
                .HasColumnName("codename");
            entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.ContentType).WithMany(p => p.AuthPermissions)
                .HasForeignKey(d => d.ContentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_permission_content_type_id_2f476e4b_fk_django_co");
        });

        modelBuilder.Entity<DTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("d_ticket_pk");

            entity.ToTable("d_ticket", tb => tb.HasComment("Талоны в очереди"));

            entity.HasIndex(e => e.Id, "d_ticket_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DTicketPrerecordId)
                .HasComment("Связь с пред записью")
                .HasColumnName("d_ticket_prerecord_id");
            entity.Property(e => e.DateRegistration)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasComment("Дата постановки в очередь")
                .HasColumnName("date_registration");
            entity.Property(e => e.SEmployeeId)
                .HasComment("Сотрудник")
                .HasColumnName("s_employee_id");
            entity.Property(e => e.SOfficeId)
                .HasComment("Офис")
                .HasColumnName("s_office_id");
            entity.Property(e => e.SOfficeTerminalId)
                .HasComment("Терминал")
                .HasColumnName("s_office_terminal_id");
            entity.Property(e => e.SOfficeWindowId)
                .HasComment("Окно")
                .HasColumnName("s_office_window_id");
            entity.Property(e => e.SPriorityId)
                .HasComment("Приоритет заявителя")
                .HasColumnName("s_priority_id");
            entity.Property(e => e.SServiceId)
                .HasComment("Услуга")
                .HasColumnName("s_service_id");
            entity.Property(e => e.SStatusId)
                .HasComment("Статус")
                .HasColumnName("s_status_id");
            entity.Property(e => e.ServicePrefix)
                .HasMaxLength(1)
                .HasComment("Префикс")
                .HasColumnName("service_prefix");
            entity.Property(e => e.TicketNumber)
                .HasComment("Номер талона")
                .HasColumnName("ticket_number");
            entity.Property(e => e.TicketNumberFull)
                .HasMaxLength(20)
                .HasComment("Полный номер талона")
                .HasColumnName("ticket_number_full");
            entity.Property(e => e.TimeRegistration)
                .HasDefaultValueSql("CURRENT_TIME")
                .HasComment("Время постановки в очередь ")
                .HasColumnName("time_registration");

            entity.HasOne(d => d.DTicketPrerecord).WithMany(p => p.DTickets)
                .HasForeignKey(d => d.DTicketPrerecordId)
                .HasConstraintName("d_ticket_fk_4");

            entity.HasOne(d => d.SEmployee).WithMany(p => p.DTickets)
                .HasForeignKey(d => d.SEmployeeId)
                .HasConstraintName("d_ticket_fk_5");

            entity.HasOne(d => d.SOffice).WithMany(p => p.DTickets)
                .HasForeignKey(d => d.SOfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_fk");

            entity.HasOne(d => d.SOfficeTerminal).WithMany(p => p.DTickets)
                .HasForeignKey(d => d.SOfficeTerminalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_fk_1");

            entity.HasOne(d => d.SOfficeWindow).WithMany(p => p.DTickets)
                .HasForeignKey(d => d.SOfficeWindowId)
                .HasConstraintName("d_ticket_fk_7");

            entity.HasOne(d => d.SPriority).WithMany(p => p.DTickets)
                .HasForeignKey(d => d.SPriorityId)
                .HasConstraintName("d_ticket_fk_3");

            entity.HasOne(d => d.SService).WithMany(p => p.DTickets)
                .HasForeignKey(d => d.SServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_fk_2");

            entity.HasOne(d => d.SStatus).WithMany(p => p.DTickets)
                .HasForeignKey(d => d.SStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_fk_6");
        });

        modelBuilder.Entity<DTicketArchve>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("d_ticket_archve_pk");

            entity.ToTable("d_ticket_archve", tb => tb.HasComment("Талоны в архиве"));

            entity.HasIndex(e => e.Id, "d_ticket_archve_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DTicketPrerecordId)
                .HasComment("Связь с предзаписью")
                .HasColumnName("d_ticket_prerecord_id");
            entity.Property(e => e.DateRegistration)
                .HasComment("Дата постановки в очередь")
                .HasColumnName("date_registration");
            entity.Property(e => e.SEmployeeId)
                .HasComment("Сотрудник")
                .HasColumnName("s_employee_id");
            entity.Property(e => e.SOfficeId)
                .HasComment("Офис")
                .HasColumnName("s_office_id");
            entity.Property(e => e.SOfficeTerminalId)
                .HasComment("Терминал")
                .HasColumnName("s_office_terminal_id");
            entity.Property(e => e.SOfficeWindowId)
                .HasComment("Окно")
                .HasColumnName("s_office_window_id");
            entity.Property(e => e.SPriorityId)
                .HasComment("Приоритет Заявителя")
                .HasColumnName("s_priority_id");
            entity.Property(e => e.SServiceId)
                .HasComment("Услуга")
                .HasColumnName("s_service_id");
            entity.Property(e => e.SStatusId)
                .HasComment("Статус")
                .HasColumnName("s_status_id");
            entity.Property(e => e.ServicePrefix)
                .HasMaxLength(1)
                .HasComment("Префикс")
                .HasColumnName("service_prefix");
            entity.Property(e => e.TicketNumber)
                .HasMaxLength(20)
                .HasComment("Номер талона")
                .HasColumnName("ticket_number");
            entity.Property(e => e.TicketNumberFull)
                .HasMaxLength(20)
                .HasComment("Полный номер талона")
                .HasColumnName("ticket_number_full");
            entity.Property(e => e.TimeCall)
                .HasComment(" Время вызова")
                .HasColumnName("time_call");
            entity.Property(e => e.TimeRegistration)
                .HasComment("Время постановки в очередь")
                .HasColumnName("time_registration");
            entity.Property(e => e.TimeStartService)
                .HasComment("Время начала обслужевания")
                .HasColumnName("time_start_service");
            entity.Property(e => e.TimeStopService)
                .HasComment("Время окончания обсужевания")
                .HasColumnName("time_stop_service");
            entity.Property(e => e.TimeWaiting)
                .HasComment("Воемя ожидания")
                .HasColumnName("time_waiting");

            entity.HasOne(d => d.DTicketPrerecord).WithMany(p => p.DTicketArchves)
                .HasForeignKey(d => d.DTicketPrerecordId)
                .HasConstraintName("d_ticket_archve_fk_3");

            entity.HasOne(d => d.SEmployee).WithMany(p => p.DTicketArchves)
                .HasForeignKey(d => d.SEmployeeId)
                .HasConstraintName("d_ticket_archve_fk");

            entity.HasOne(d => d.SOffice).WithMany(p => p.DTicketArchves)
                .HasForeignKey(d => d.SOfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_archve_fk_6");

            entity.HasOne(d => d.SOfficeTerminal).WithMany(p => p.DTicketArchves)
                .HasForeignKey(d => d.SOfficeTerminalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_archve_fk_7");

            entity.HasOne(d => d.SOfficeWindow).WithMany(p => p.DTicketArchves)
                .HasForeignKey(d => d.SOfficeWindowId)
                .HasConstraintName("d_ticket_archve_fk_1");

            entity.HasOne(d => d.SPriority).WithMany(p => p.DTicketArchves)
                .HasForeignKey(d => d.SPriorityId)
                .HasConstraintName("d_ticket_archve_fk_4");

            entity.HasOne(d => d.SService).WithMany(p => p.DTicketArchves)
                .HasForeignKey(d => d.SServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_archve_fk_5");

            entity.HasOne(d => d.SStatus).WithMany(p => p.DTicketArchves)
                .HasForeignKey(d => d.SStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_archve_fk_2");
        });

        modelBuilder.Entity<DTicketPrerecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("d_ticket_prerecord_pk");

            entity.ToTable("d_ticket_prerecord", tb => tb.HasComment("Талоны предзаписи"));

            entity.HasIndex(e => e.Id, "d_ticket_prerecord_id_idx");

            entity.Property(e => e.Id)
                .HasComment(" Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CodePrerecord)
                .HasComment("Код предзаписи")
                .HasColumnName("code_prerecord");
            entity.Property(e => e.CustomerEMail)
                .HasMaxLength(70)
                .HasComment("Почта заявителя")
                .HasColumnName("customer_e_mail");
            entity.Property(e => e.CustomerFullName)
                .HasMaxLength(70)
                .HasComment("ФИО")
                .HasColumnName("customer_full_name");
            entity.Property(e => e.CustomerPhoneNumber)
                .HasMaxLength(20)
                .HasComment("Номер телефона заявителя")
                .HasColumnName("customer_phone_number");
            entity.Property(e => e.CustomerSnils)
                .HasMaxLength(20)
                .HasComment("СНИЛС Заявителя")
                .HasColumnName("customer_snils");
            entity.Property(e => e.DataAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления записи")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_add");
            entity.Property(e => e.DatePrerecord)
                .HasComment("Дата предзаписи")
                .HasColumnName("date_prerecord");
            entity.Property(e => e.IsConfirmation)
                .HasComment("Подтвреждение того что заявитель пришел и встал в очередь")
                .HasColumnName("is_confirmation");
            entity.Property(e => e.SEmployeeId)
                .HasComment("Сотрудник")
                .HasColumnName("s_employee_id");
            entity.Property(e => e.SOfficeId)
                .HasComment("Офис")
                .HasColumnName("s_office_id");
            entity.Property(e => e.SServiceId)
                .HasComment("Услуга")
                .HasColumnName("s_service_id");
            entity.Property(e => e.SSourсePrerecordId)
                .HasComment("Источник")
                .HasColumnName("s_sourсe_prerecord_id");
            entity.Property(e => e.StartTimePrerecord)
                .HasComment("Начала время предзаписи")
                .HasColumnName("start_time_prerecord");
            entity.Property(e => e.StopTimePrerecord)
                .HasComment("Окончание времени предзаписи")
                .HasColumnName("stop_time_prerecord");

            entity.HasOne(d => d.SEmployee).WithMany(p => p.DTicketPrerecords)
                .HasForeignKey(d => d.SEmployeeId)
                .HasConstraintName("d_ticket_prerecord_fk");

            entity.HasOne(d => d.SOffice).WithMany(p => p.DTicketPrerecords)
                .HasForeignKey(d => d.SOfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_prerecord_fk_2");

            entity.HasOne(d => d.SService).WithMany(p => p.DTicketPrerecords)
                .HasForeignKey(d => d.SServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_prerecord_fk_3");

            entity.HasOne(d => d.SSourсePrerecord).WithMany(p => p.DTicketPrerecords)
                .HasForeignKey(d => d.SSourсePrerecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_prerecord_fk_1");
        });

        modelBuilder.Entity<DTicketStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("d_ticket_status_pk");

            entity.ToTable("d_ticket_status", tb => tb.HasComment("Статусы талонов в очереди"));

            entity.HasIndex(e => e.Id, "d_ticket_status_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DTicketId)
                .HasComment(" Талон")
                .HasColumnName("d_ticket_id");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasComment("Дата")
                .HasColumnName("date_add");
            entity.Property(e => e.SEmployeeId)
                .HasComment("Сотрудник")
                .HasColumnName("s_employee_id");
            entity.Property(e => e.SOfficeWindowId)
                .HasComment("Окно")
                .HasColumnName("s_office_window_id");
            entity.Property(e => e.SOfficeWindowIdRedirect)
                .HasComment("Окно куда перенаправили")
                .HasColumnName("s_office_window_id_redirect");
            entity.Property(e => e.SStatusId)
                .HasComment("Статус")
                .HasColumnName("s_status_id");
            entity.Property(e => e.TimeAdd)
                .HasDefaultValueSql("CURRENT_TIME")
                .HasComment("Время")
                .HasColumnName("time_add");

            entity.HasOne(d => d.DTicket).WithMany(p => p.DTicketStatuses)
                .HasForeignKey(d => d.DTicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_status_fk_3");

            entity.HasOne(d => d.SEmployee).WithMany(p => p.DTicketStatuses)
                .HasForeignKey(d => d.SEmployeeId)
                .HasConstraintName("d_ticket_status_fk_1");

            entity.HasOne(d => d.SOfficeWindow).WithMany(p => p.DTicketStatusSOfficeWindows)
                .HasForeignKey(d => d.SOfficeWindowId)
                .HasConstraintName("d_ticket_status_fk_2");

            entity.HasOne(d => d.SOfficeWindowIdRedirectNavigation).WithMany(p => p.DTicketStatusSOfficeWindowIdRedirectNavigations)
                .HasForeignKey(d => d.SOfficeWindowIdRedirect)
                .HasConstraintName("d_ticket_status_fk_4");

            entity.HasOne(d => d.SStatus).WithMany(p => p.DTicketStatuses)
                .HasForeignKey(d => d.SStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_status_fk");
        });

        modelBuilder.Entity<DTicketStatusArchve>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("d_ticket_status_archve_pk");

            entity.ToTable("d_ticket_status_archve", tb => tb.HasComment("Статусы талонов в архиве"));

            entity.HasIndex(e => e.Id, "d_ticket_status_archve_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DTicketArchiveId)
                .HasComment("Талон")
                .HasColumnName("d_ticket_archive_id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата")
                .HasColumnName("date_add");
            entity.Property(e => e.SEmployeeId)
                .HasComment("Сотрудник")
                .HasColumnName("s_employee_id");
            entity.Property(e => e.SOfficeWindowId)
                .HasComment("Окно")
                .HasColumnName("s_office_window_id");
            entity.Property(e => e.SOfficeWindowIdRedirect)
                .HasComment("Окно куда перенаправили")
                .HasColumnName("s_office_window_id_redirect");
            entity.Property(e => e.SStatusId)
                .HasComment("Статус")
                .HasColumnName("s_status_id");
            entity.Property(e => e.TimeAdd)
                .HasComment(" Время")
                .HasColumnName("time_add");

            entity.HasOne(d => d.DTicketArchive).WithMany(p => p.DTicketStatusArchves)
                .HasForeignKey(d => d.DTicketArchiveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_status_archve_fk_4");

            entity.HasOne(d => d.SEmployee).WithMany(p => p.DTicketStatusArchves)
                .HasForeignKey(d => d.SEmployeeId)
                .HasConstraintName("d_ticket_status_archve_fk_2");

            entity.HasOne(d => d.SOfficeWindow).WithMany(p => p.DTicketStatusArchveSOfficeWindows)
                .HasForeignKey(d => d.SOfficeWindowId)
                .HasConstraintName("d_ticket_status_archve_fk");

            entity.HasOne(d => d.SOfficeWindowIdRedirectNavigation).WithMany(p => p.DTicketStatusArchveSOfficeWindowIdRedirectNavigations)
                .HasForeignKey(d => d.SOfficeWindowIdRedirect)
                .HasConstraintName("d_ticket_status_archve_fk_1");

            entity.HasOne(d => d.SStatus).WithMany(p => p.DTicketStatusArchves)
                .HasForeignKey(d => d.SStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("d_ticket_status_archve_fk_3");
        });

        modelBuilder.Entity<DjangoAdminLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("django_admin_log_pkey");

            entity.ToTable("django_admin_log");

            entity.HasIndex(e => e.ContentTypeId, "django_admin_log_content_type_id_c4bce8eb");

            entity.HasIndex(e => e.UserId, "django_admin_log_user_id_c564eba6");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionFlag).HasColumnName("action_flag");
            entity.Property(e => e.ActionTime).HasColumnName("action_time");
            entity.Property(e => e.ChangeMessage).HasColumnName("change_message");
            entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");
            entity.Property(e => e.ObjectId).HasColumnName("object_id");
            entity.Property(e => e.ObjectRepr)
                .HasMaxLength(200)
                .HasColumnName("object_repr");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ContentType).WithMany(p => p.DjangoAdminLogs)
                .HasForeignKey(d => d.ContentTypeId)
                .HasConstraintName("django_admin_log_content_type_id_c4bce8eb_fk_django_co");

            entity.HasOne(d => d.User).WithMany(p => p.DjangoAdminLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("django_admin_log_user_id_c564eba6_fk_users_id");
        });

        modelBuilder.Entity<DjangoContentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("django_content_type_pkey");

            entity.ToTable("django_content_type");

            entity.HasIndex(e => new { e.AppLabel, e.Model }, "django_content_type_app_label_model_76bd3d3b_uniq").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppLabel)
                .HasMaxLength(100)
                .HasColumnName("app_label");
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .HasColumnName("model");
        });

        modelBuilder.Entity<DjangoMigration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("django_migrations_pkey");

            entity.ToTable("django_migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.App)
                .HasMaxLength(255)
                .HasColumnName("app");
            entity.Property(e => e.Applied).HasColumnName("applied");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DjangoSession>(entity =>
        {
            entity.HasKey(e => e.SessionKey).HasName("django_session_pkey");

            entity.ToTable("django_session");

            entity.HasIndex(e => e.ExpireDate, "django_session_expire_date_a5c62663");

            entity.HasIndex(e => e.SessionKey, "django_session_session_key_c0390e0f_like").HasOperators(new[] { "varchar_pattern_ops" });

            entity.Property(e => e.SessionKey)
                .HasMaxLength(40)
                .HasColumnName("session_key");
            entity.Property(e => e.ExpireDate).HasColumnName("expire_date");
            entity.Property(e => e.SessionData).HasColumnName("session_data");
        });

        modelBuilder.Entity<SDayWeek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_day_week_pk");

            entity.ToTable("s_day_week", tb => tb.HasComment("Справочник дней недели"));

            entity.HasIndex(e => e.Id, "s_day_week_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DayName)
                .HasMaxLength(30)
                .HasComment("Наименование")
                .HasColumnName("day_name");
        });

        modelBuilder.Entity<SEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_employee_pk");

            entity.ToTable("s_employee", tb => tb.HasComment("Справочник сотрудников"));

            entity.HasIndex(e => e.Id, "s_employee_id_idx");

            entity.HasIndex(e => e.SOfficeId, "s_employee_s_office_id_idx");

            entity.HasIndex(e => e.SRoleId, "s_employee_s_role_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment(" Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.FullName)
                .HasMaxLength(70)
                .HasComment("ФИО")
                .HasColumnName("full_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasComment("Номер телефона")
                .HasColumnName("phone_number");
            entity.Property(e => e.SOfficeId)
                .HasComment("Офис")
                .HasColumnName("s_office_id");
            entity.Property(e => e.SRoleId)
                .HasComment("Роль")
                .HasColumnName("s_role_id");
            entity.Property(e => e.UserId)
                .HasComment("Пользователь")
                .HasColumnName("user_id");

            entity.HasOne(d => d.SOffice).WithMany(p => p.SEmployees)
                .HasForeignKey(d => d.SOfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_employee_fk");

            entity.HasOne(d => d.SRole).WithMany(p => p.SEmployees)
                .HasForeignKey(d => d.SRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_employee_fk_1");
        });

        modelBuilder.Entity<SOffice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_pk");

            entity.ToTable("s_office", tb => tb.HasComment("Справочник офисов"));

            entity.HasIndex(e => e.Id, "s_office_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CountDayPrerecord)
                .HasComment("Количество дней для перезаписи")
                .HasColumnName("count_day_prerecord");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.OfficeAddress)
                .HasMaxLength(255)
                .HasComment("Адрес")
                .HasColumnName("office_address");
            entity.Property(e => e.OfficeName)
                .HasMaxLength(255)
                .HasComment("Наименование")
                .HasColumnName("office_name");
        });

        modelBuilder.Entity<SOfficePrerecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_prerecord_pk");

            entity.ToTable("s_office_prerecord", tb => tb.HasComment("Справочник ограничения по количеству талонов офисов"));

            entity.HasIndex(e => e.Id, "s_office_prerecord_id_idx").IsUnique();

            entity.HasIndex(e => e.SOfficeId, "s_office_prerecord_idx2");

            entity.HasIndex(e => e.SDayWeekId, "s_office_prerecord_idx3");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.PrerecordCount)
                .HasComment("Количество талонов ")
                .HasColumnName("prerecord_count");
            entity.Property(e => e.SDayWeekId)
                .HasComment("День недели")
                .HasColumnName("s_day_week_id");
            entity.Property(e => e.SOfficeId)
                .HasComment("Офис")
                .HasColumnName("s_office_id");
            entity.Property(e => e.StartTimePrerecord)
                .HasComment("Начала время предзаписи")
                .HasColumnName("start_time_prerecord");
            entity.Property(e => e.StopTimePrerecord)
                .HasComment("Окончание времени предзаписи")
                .HasColumnName("stop_time_prerecord");

            entity.HasOne(d => d.SOffice).WithMany(p => p.SOfficePrerecords)
                .HasForeignKey(d => d.SOfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_prerecord_fk");
        });

        modelBuilder.Entity<SOfficeSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_schedule_pk");

            entity.ToTable("s_office_schedule", tb => tb.HasComment("Справочник графиков работ офисов"));

            entity.HasIndex(e => e.Id, "s_office_schedule_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.SDayWeekId)
                .HasComment("День недели")
                .HasColumnName("s_day_week_id");
            entity.Property(e => e.SOfficeId)
                .HasComment("Офис")
                .HasColumnName("s_office_id");
            entity.Property(e => e.StartTime)
                .HasComment("Время начала")
                .HasColumnName("start_time");
            entity.Property(e => e.StopTime)
                .HasComment(" Время окончание")
                .HasColumnName("stop_time");

            entity.HasOne(d => d.SDayWeek).WithMany(p => p.SOfficeSchedules)
                .HasForeignKey(d => d.SDayWeekId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_schedule_fk_1");

            entity.HasOne(d => d.SOffice).WithMany(p => p.SOfficeSchedules)
                .HasForeignKey(d => d.SOfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_schedule_fk");
        });

        modelBuilder.Entity<SOfficeScoreboard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_scoreboard_pk");

            entity.ToTable("s_office_scoreboard", tb => tb.HasComment("Справочник информационных табло офисов"));

            entity.HasIndex(e => e.Id, "s_office_scoreboard_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Ключ")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Дата и время добавления ")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.IsActive)
                .HasComment("Активность")
                .HasColumnName("is_active");
            entity.Property(e => e.SOfficeId)
                .HasComment("Офис")
                .HasColumnName("s_office_id");
            entity.Property(e => e.ScoreboardName)
                .HasMaxLength(70)
                .HasComment("Наименование")
                .HasColumnName("scoreboard_name");

            entity.HasOne(d => d.SOffice).WithMany(p => p.SOfficeScoreboards)
                .HasForeignKey(d => d.SOfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_scoreboard_fk");
        });

        modelBuilder.Entity<SOfficeScoreboardMultimedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_scoreboard_multimedia_pk");

            entity.ToTable("s_office_scoreboard_multimedia", tb => tb.HasComment("Справочник фото видео табло офиса"));

            entity.HasIndex(e => e.Id, "s_office_scoreboard_multimedia_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.IsActive)
                .HasComment("Активность")
                .HasColumnName("is_active");
            entity.Property(e => e.MultimediaName)
                .HasMaxLength(255)
                .HasComment("Наименование")
                .HasColumnName("multimedia_name");
            entity.Property(e => e.MultimediaPath)
                .HasMaxLength(255)
                .HasComment("Путь к фото видео")
                .HasColumnName("multimedia_path");
            entity.Property(e => e.SOfficeScoreboardId)
                .HasComment("Табло")
                .HasColumnName("s_office_scoreboard_id");

            entity.HasOne(d => d.SOfficeScoreboard).WithMany(p => p.SOfficeScoreboardMultimedia)
                .HasForeignKey(d => d.SOfficeScoreboardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_scoreboard_multimedia_fk");
        });

        modelBuilder.Entity<SOfficeScoreboardService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_scoreboard_service_pk");

            entity.ToTable("s_office_scoreboard_service", tb => tb.HasComment("Справочник услуг табло офиса"));

            entity.HasIndex(e => e.Id, "s_office_scoreboard_service_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.SOfficeScoreboardId)
                .HasComment("Табло")
                .HasColumnName("s_office_scoreboard_id");
            entity.Property(e => e.SServiceId)
                .HasComment("Услуга")
                .HasColumnName("s_service_id");

            entity.HasOne(d => d.SOfficeScoreboard).WithMany(p => p.SOfficeScoreboardServices)
                .HasForeignKey(d => d.SOfficeScoreboardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_scoreboard_service_fk_1");

            entity.HasOne(d => d.SService).WithMany(p => p.SOfficeScoreboardServices)
                .HasForeignKey(d => d.SServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_scoreboard_service_fk");
        });

        modelBuilder.Entity<SOfficeTerminal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_terminal_pk");

            entity.ToTable("s_office_terminal", tb => tb.HasComment("Справочник терминалов офисов"));

            entity.HasIndex(e => e.Id, "s_office_terminal_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления ")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment(" Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(70)
                .HasComment("IP Адрес")
                .HasColumnName("ip_address");
            entity.Property(e => e.NumberColumns)
                .HasComment("Количество столбцов")
                .HasColumnName("number_columns");
            entity.Property(e => e.NumberLines)
                .HasComment("Количество строк")
                .HasColumnName("number_lines");
            entity.Property(e => e.SOfficeId)
                .HasComment("Офис")
                .HasColumnName("s_office_id");
            entity.Property(e => e.TerminalName)
                .HasMaxLength(70)
                .HasComment(" Наименование")
                .HasColumnName("terminal_name");

            entity.HasOne(d => d.SOffice).WithMany(p => p.SOfficeTerminals)
                .HasForeignKey(d => d.SOfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_terminal_fk");
        });

        modelBuilder.Entity<SOfficeTerminalButton>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_terminal_folder_pk");

            entity.ToTable("s_office_terminal_button", tb => tb.HasComment("Справочник кнопок терминала офиса"));

            entity.HasIndex(e => e.Id, "s_office_terminal_folder_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ButtonName)
                .HasMaxLength(70)
                .HasComment("Наименование")
                .HasColumnName("button_name");
            entity.Property(e => e.ButtonType)
                .HasComment("Тип. 1 - Меню 2 - Услуга")
                .HasColumnName("button_type");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.ParentId)
                .HasComment("Родительская запись")
                .HasColumnName("parent_id");
            entity.Property(e => e.SOfficeTerminalId)
                .HasComment("Терминал")
                .HasColumnName("s_office_terminal_id");
            entity.Property(e => e.SServiceId)
                .HasComment("Услуга")
                .HasColumnName("s_service_id");

            entity.HasOne(d => d.SOfficeTerminal).WithMany(p => p.SOfficeTerminalButtons)
                .HasForeignKey(d => d.SOfficeTerminalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_terminal_folder_fk");
        });

        modelBuilder.Entity<SOfficeTerminalService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_terminal_service_pk");

            entity.ToTable("s_office_terminal_service", tb => tb.HasComment("Справочник услуг терминала офиса"));

            entity.HasIndex(e => e.Id, "s_office_terminal_service_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.IsActive)
                .HasComment("Активность")
                .HasColumnName("is_active");
            entity.Property(e => e.SOfficeTerminalFolderId)
                .HasComment("Папка")
                .HasColumnName("s_office_terminal_folder_id");
            entity.Property(e => e.SOfficeTerminalId)
                .HasComment("Терминал")
                .HasColumnName("s_office_terminal_id");
            entity.Property(e => e.SServiceId)
                .HasComment("Услуга")
                .HasColumnName("s_service_id");

            entity.HasOne(d => d.SOfficeTerminalFolder).WithMany(p => p.SOfficeTerminalServices)
                .HasForeignKey(d => d.SOfficeTerminalFolderId)
                .HasConstraintName("s_office_terminal_service_fk_2");

            entity.HasOne(d => d.SOfficeTerminal).WithMany(p => p.SOfficeTerminalServices)
                .HasForeignKey(d => d.SOfficeTerminalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_terminal_service_fk_1");

            entity.HasOne(d => d.SService).WithMany(p => p.SOfficeTerminalServices)
                .HasForeignKey(d => d.SServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_terminal_service_fk");
        });

        modelBuilder.Entity<SOfficeWindow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_window_pk");

            entity.ToTable("s_office_window", tb => tb.HasComment("Справочник окон офисов"));

            entity.HasIndex(e => e.Id, "s_office_window_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.SOfficeId)
                .HasComment("Офис")
                .HasColumnName("s_office_id");
            entity.Property(e => e.WindowIp)
                .HasMaxLength(20)
                .HasComment("IP Адрес")
                .HasColumnName("window_ip");
            entity.Property(e => e.WindowName)
                .HasMaxLength(70)
                .HasComment("Наименование ")
                .HasColumnName("window_name");

            entity.HasOne(d => d.SOffice).WithMany(p => p.SOfficeWindows)
                .HasForeignKey(d => d.SOfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_window_fk");
        });

        modelBuilder.Entity<SOfficeWindowService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_office_window_service_pk");

            entity.ToTable("s_office_window_service", tb => tb.HasComment("Справочник услуг окна офиса"));

            entity.HasIndex(e => e.Id, "s_office_window_service_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.IsActive)
                .HasComment("Активность")
                .HasColumnName("is_active");
            entity.Property(e => e.SOfficeWindowId)
                .HasComment("Окно")
                .HasColumnName("s_office_window_id");
            entity.Property(e => e.SServiceId)
                .HasComment("Услуга")
                .HasColumnName("s_service_id");

            entity.HasOne(d => d.SOfficeWindow).WithMany(p => p.SOfficeWindowServices)
                .HasForeignKey(d => d.SOfficeWindowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_window_service_fk");

            entity.HasOne(d => d.SService).WithMany(p => p.SOfficeWindowServices)
                .HasForeignKey(d => d.SServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s_office_window_service_fk_1");
        });

        modelBuilder.Entity<SPriority>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_priority_pk");

            entity.ToTable("s_priority", tb => tb.HasComment("Справочник приоритетов"));

            entity.HasIndex(e => e.Id, "s_priority_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Commentt)
                .HasMaxLength(500)
                .HasComment("Комментарий")
                .HasColumnName("commentt");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("now()")
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.IsActive)
                .HasComment("Активность")
                .HasColumnName("is_active");
            entity.Property(e => e.PriorityName)
                .HasMaxLength(70)
                .HasComment("Наименование")
                .HasColumnName("priority_name");
            entity.Property(e => e.PriorityPosition)
                .HasComment("Пазиция приоритета")
                .HasColumnName("priority_position");
        });

        modelBuilder.Entity<SRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_role_pk");

            entity.ToTable("s_role", tb => tb.HasComment("Справочник ролей"));

            entity.HasIndex(e => e.Id, "s_role_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .HasComment("Наименование")
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<SService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_service_pk");

            entity.ToTable("s_service", tb => tb.HasComment("Справочник услуг"));

            entity.HasIndex(e => e.Id, "s_service_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(20)
                .HasComment("Наименование")
                .HasColumnName("service_name");
            entity.Property(e => e.ServicePrefix)
                .HasMaxLength(1)
                .HasComment("Префикс")
                .HasColumnName("service_prefix");
            entity.Property(e => e.ServicePriority)
                .HasComment("Прироитет ")
                .HasColumnName("service_priority");
        });

        modelBuilder.Entity<SSourcePrerecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_sourse_prerecord_pk");

            entity.ToTable("s_source_prerecord", tb => tb.HasComment("Справочник источников пред записи"));

            entity.HasIndex(e => e.Id, "s_sourse_prerecord_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.PrerecordName)
                .HasMaxLength(30)
                .HasComment("Наименование")
                .HasColumnName("prerecord_name");
        });

        modelBuilder.Entity<SStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_status_pk");

            entity.ToTable("s_status", tb => tb.HasComment("Справочник статусов талонов"));

            entity.HasIndex(e => e.Id, "s_status_id_idx");

            entity.Property(e => e.Id)
                .HasComment(" Наименование")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(30)
                .HasComment("Наименование")
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<SWeekend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s_weekend_pk");

            entity.ToTable("s_weekend", tb => tb.HasComment("Календарь выходных дней"));

            entity.HasIndex(e => e.Id, "s_weekend_id_idx");

            entity.Property(e => e.Id)
                .HasComment("Ключ")
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DateAdd)
                .HasComment("Дата и время добавления")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.DateWeekend)
                .HasComment("Дата")
                .HasColumnName("date_weekend");
            entity.Property(e => e.EmployeeNameAdd)
                .HasMaxLength(70)
                .HasComment("Кто добавил")
                .HasColumnName("employee_name_add");
        });

        modelBuilder.Entity<ThumbnailKvstore>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("thumbnail_kvstore_pkey");

            entity.ToTable("thumbnail_kvstore");

            entity.HasIndex(e => e.Key, "thumbnail_kvstore_key_3f850178_like").HasOperators(new[] { "varchar_pattern_ops" });

            entity.Property(e => e.Key)
                .HasMaxLength(200)
                .HasColumnName("key");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "users_username_e8658fc8_like").HasOperators(new[] { "varchar_pattern_ops" });

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateJoined).HasColumnName("date_joined");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .HasColumnName("first_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsStaff).HasColumnName("is_staff");
            entity.Property(e => e.IsSuperuser).HasColumnName("is_superuser");
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(150)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UsersGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_groups_pkey");

            entity.ToTable("users_groups");

            entity.HasIndex(e => e.GroupId, "users_groups_group_id_2f3517aa");

            entity.HasIndex(e => e.UserId, "users_groups_user_id_f500bee5");

            entity.HasIndex(e => new { e.UserId, e.GroupId }, "users_groups_user_id_group_id_fc7788e8_uniq").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Group).WithMany(p => p.UsersGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_groups_group_id_2f3517aa_fk_auth_group_id");

            entity.HasOne(d => d.User).WithMany(p => p.UsersGroups)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_groups_user_id_f500bee5_fk_users_id");
        });

        modelBuilder.Entity<UsersUserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_user_permissions_pkey");

            entity.ToTable("users_user_permissions");

            entity.HasIndex(e => e.PermissionId, "users_user_permissions_permission_id_6d08dcd2");

            entity.HasIndex(e => e.UserId, "users_user_permissions_user_id_92473840");

            entity.HasIndex(e => new { e.UserId, e.PermissionId }, "users_user_permissions_user_id_permission_id_3b86cbdf_uniq").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Permission).WithMany(p => p.UsersUserPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_user_permissio_permission_id_6d08dcd2_fk_auth_perm");

            entity.HasOne(d => d.User).WithMany(p => p.UsersUserPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_user_permissions_user_id_92473840_fk_users_id");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
