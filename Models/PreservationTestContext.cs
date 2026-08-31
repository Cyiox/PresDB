using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebPresDB.Models;

public partial class PreservationTestContext : DbContext
{
    public PreservationTestContext()
    {
    }

    public PreservationTestContext(DbContextOptions<PreservationTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AtRiskReportDataArchive> AtRiskReportDataArchives { get; set; }

    public virtual DbSet<DataSourceConfiguration> DataSourceConfigurations { get; set; }

    public virtual DbSet<DebugLog> DebugLogs { get; set; }

    public virtual DbSet<DhcdArchive> DhcdArchives { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<ImportDataIssue> ImportDataIssues { get; set; }

    public virtual DbSet<ImportDataRule> ImportDataRules { get; set; }

    public virtual DbSet<ImportLog> ImportLogs { get; set; }

    public virtual DbSet<LihtcpubArchive> LihtcpubArchives { get; set; }

    public virtual DbSet<MassHousingBenedictArchive> MassHousingBenedictArchives { get; set; }

    public virtual DbSet<MassHousingFoaArchive> MassHousingFoaArchives { get; set; }

    public virtual DbSet<MfAssistanceSec8ContractsArchive> MfAssistanceSec8ContractsArchives { get; set; }

    public virtual DbSet<MfPropertiesWithAssistanceSec8ContractsArchive> MfPropertiesWithAssistanceSec8ContractsArchives { get; set; }

    public virtual DbSet<MhpArchive> MhpArchives { get; set; }

    public virtual DbSet<MtgAArchive> MtgAArchives { get; set; }

    public virtual DbSet<MtgTArchive> MtgTArchives { get; set; }

    public virtual DbSet<PreservedUnitsAging> PreservedUnitsAgings { get; set; }

    public virtual DbSet<PropertiesExcludedFromAtRiskReport> PropertiesExcludedFromAtRiskReports { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<Tbl40Tdocument> Tbl40Tdocuments { get; set; }

    public virtual DbSet<UsdaArchive> UsdaArchives { get; set; }

    public virtual DbSet<UspsAddress> UspsAddresses { get; set; }

    public virtual DbSet<XS8> XS8s { get; set; }

    public virtual DbSet<ZAtRiskReportExclusionStatus> ZAtRiskReportExclusionStatuses { get; set; }

    public virtual DbSet<ZCity> ZCities { get; set; }

    public virtual DbSet<ZDocumentType> ZDocumentTypes { get; set; }

    public virtual DbSet<ZLogoutUser> ZLogoutUsers { get; set; }

    public virtual DbSet<ZPreservedByProgram> ZPreservedByPrograms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // leave this blank or use fallback only for design-time scenarios
            // optionsBuilder.UseSqlServer("YourFallbackConnectionString");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AtRiskReportDataArchive>(entity =>
        {
            entity.ToTable("AtRiskReportData_archive");

            entity.HasIndex(e => e.PropertyId, "PropertyID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Agency).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.LocalUseRestrictionNotes).HasMaxLength(255);
            entity.Property(e => e.PreservedByProgram).HasMaxLength(255);
            entity.Property(e => e.PreservedThroughDate).HasColumnType("datetime");
            entity.Property(e => e.PropertyId).HasColumnName("PropertyID");
            entity.Property(e => e.PropertyName).HasMaxLength(255);
            entity.Property(e => e.ReportAtRiskDate)
                .HasColumnType("datetime")
                .HasColumnName("report_AtRiskDate");
            entity.Property(e => e.ReportName)
                .HasMaxLength(255)
                .HasColumnName("report_Name");
            entity.Property(e => e.ReportRunDateTime)
                .HasColumnType("datetime")
                .HasColumnName("report_RunDateTime");
            entity.Property(e => e.ReportUsername)
                .HasMaxLength(255)
                .HasColumnName("Report_Username");
            entity.Property(e => e.S8ExpDate)
                .HasColumnType("datetime")
                .HasColumnName("S8_ExpDate");
            entity.Property(e => e.S8Pbaunits).HasColumnName("S8_PBAUnits");
            entity.Property(e => e.Street).HasMaxLength(255);
            entity.Property(e => e.UnitsAtRiskStatus).HasMaxLength(255);
            entity.Property(e => e.Zip).HasMaxLength(25);
        });

        modelBuilder.Entity<DataSourceConfiguration>(entity =>
        {
            entity.HasKey(e => e.DataSource);

            entity.ToTable("DataSourceConfiguration");

            entity.HasIndex(e => e.ImportIdfieldName, "IDFieldName");

            entity.Property(e => e.DataSource).HasMaxLength(255);
            entity.Property(e => e.AppendCriteria).HasMaxLength(255);
            entity.Property(e => e.AppendFromFieldReplacements).HasMaxLength(2000);
            entity.Property(e => e.AppendIntoFieldReplacements).HasMaxLength(2000);
            entity.Property(e => e.DataFileExtension).HasMaxLength(10);
            entity.Property(e => e.DataFileVerificationWord).HasMaxLength(25);
            entity.Property(e => e.DataSourceDesc).HasMaxLength(255);
            entity.Property(e => e.DataSourceTitle).HasMaxLength(255);
            entity.Property(e => e.DbtableName)
                .HasMaxLength(255)
                .HasColumnName("DBTableName");
            entity.Property(e => e.DetailsSubformName).HasMaxLength(255);
            entity.Property(e => e.ExpectedFieldList).HasMaxLength(2000);
            entity.Property(e => e.FieldsToIgnoreForDiff).HasMaxLength(255);
            entity.Property(e => e.ImportIdfieldName)
                .HasMaxLength(255)
                .HasColumnName("ImportIDFieldName");
            entity.Property(e => e.ImportIdfieldType)
                .HasMaxLength(255)
                .HasColumnName("ImportIDFieldType");
            entity.Property(e => e.LinkedTableName).HasMaxLength(255);
            entity.Property(e => e.MatchingFormSql)
                .HasMaxLength(2000)
                .HasColumnName("MatchingFormSQL");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.SourceTableName).HasMaxLength(255);
        });

        modelBuilder.Entity<DebugLog>(entity =>
        {
            entity.ToTable("DebugLog");

            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemDate).HasColumnType("datetime");
            entity.Property(e => e.ItemDescription).HasMaxLength(255);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.User).HasMaxLength(100);
        });

        modelBuilder.Entity<DhcdArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("DHCD_archive");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => e.NumberOfAffordableUnits, "Number_of_Affordable_Units");

            entity.HasIndex(e => e.NumberOfExtremelyLowIncomeUnits, "Number_of_Extremely_Low_Income_Units");

            entity.HasIndex(e => e.NumberOfTotalUnits, "Number_of_Total_Units");

            entity.HasIndex(e => e.OldProjId, "Old_Proj_ID");

            entity.HasIndex(e => e.ProjId, "Proj_ID");

            entity.HasIndex(e => e.ImportDateTime, "archive_date_time");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.AcquisitionCreditPisDate)
                .HasColumnType("datetime")
                .HasColumnName("Acquisition Credit PIS Date");
            entity.Property(e => e.AhtfAwarded)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("AHTF Awarded");
            entity.Property(e => e.BorrowerSponsor)
                .HasMaxLength(255)
                .HasColumnName("Borrower Sponsor");
            entity.Property(e => e.CatnhpAwarded).HasColumnName("CATNHP Awarded");
            entity.Property(e => e.CbhAwarded)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("CBH Awarded");
            entity.Property(e => e.CipfAwarded).HasColumnName("CIPF Awarded");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.DateCatnhpLoanClosed)
                .HasColumnType("datetime")
                .HasColumnName("Date CATNHP Loan Closed");
            entity.Property(e => e.DateCbhLoanClosed)
                .HasColumnType("datetime")
                .HasColumnName("Date CBH Loan Closed");
            entity.Property(e => e.DateCipfLoanClosed)
                .HasColumnType("datetime")
                .HasColumnName("Date CIPF Loan Closed");
            entity.Property(e => e.DateFcfDdsLoanClosed)
                .HasColumnType("datetime")
                .HasColumnName("Date FCF DDS Loan Closed");
            entity.Property(e => e.DateFcfDmhLoanClosed)
                .HasColumnType("datetime")
                .HasColumnName("Date FCF DMH Loan Closed");
            entity.Property(e => e.DateHifLoanClosed)
                .HasColumnType("datetime")
                .HasColumnName("Date HIF Loan Closed");
            entity.Property(e => e.DateHomeLoanClosed)
                .HasColumnType("datetime")
                .HasColumnName("Date HOME Loan Closed");
            entity.Property(e => e.DateHsfLoanClosed)
                .HasColumnType("datetime")
                .HasColumnName("Date HSF Loan Closed");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.FcfDdsAwarded)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("FCF DDS Awarded");
            entity.Property(e => e.FcfDmhAwarded)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("FCF DMH Awarded");
            entity.Property(e => e.FourPctAwarded)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("Four Pct Awarded");
            entity.Property(e => e.HifAwarded)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("HIF Awarded");
            entity.Property(e => e.HomeAwarded)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("Home Awarded");
            entity.Property(e => e.HpstfAwarded).HasColumnName("HPSTF Awarded");
            entity.Property(e => e.HsfAwarded)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("HSF Awarded");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.NhtfAwarded)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("NHTF Awarded");
            entity.Property(e => e.NinePctAwarded).HasColumnName("Nine Pct Awarded");
            entity.Property(e => e.NumberOfAffordableUnits).HasColumnName("Number of Affordable Units");
            entity.Property(e => e.NumberOfExtremelyLowIncomeUnits).HasColumnName("Number of Extremely Low Income Units");
            entity.Property(e => e.NumberOfTotalUnits).HasColumnName("Number of Total Units");
            entity.Property(e => e.OldProjId).HasColumnName("Old Proj ID");
            entity.Property(e => e.Pre2016Pis)
                .HasColumnType("datetime")
                .HasColumnName("Pre2016_PIS");
            entity.Property(e => e.Pre2016SlihtcState).HasColumnName("Pre2016_SLIHTC(State)");
            entity.Property(e => e.Pre2016Total).HasColumnName("Pre2016_Total");
            entity.Property(e => e.ProjId)
                .HasMaxLength(255)
                .HasColumnName("Proj ID");
            entity.Property(e => e.ProjectAddress)
                .HasMaxLength(255)
                .HasColumnName("Project Address");
            entity.Property(e => e.ProjectName).HasMaxLength(255);
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.RehabCreditPisDate)
                .HasColumnType("datetime")
                .HasColumnName("Rehab Credit PIS Date");
            entity.Property(e => e.StateLihtcAwarded).HasColumnName("State LIHTC Awarded");
            entity.Property(e => e.TcapAwarded).HasColumnName("TCAP Awarded");
            entity.Property(e => e.TcxAwarded).HasColumnName("TCX Awarded");
            entity.Property(e => e.TodAwarded).HasColumnName("TOD Awarded");
            entity.Property(e => e.ZipCode4)
                .HasMaxLength(255)
                .HasColumnName("Zip Code +4");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.DhcdArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("PropertiesDHCD_archive");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasIndex(e => e.PropertyId, "PropertyID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddDate)
                .HasColumnType("datetime")
                .HasColumnName("add_date");
            entity.Property(e => e.AddUser)
                .HasMaxLength(50)
                .HasColumnName("add_user");
            entity.Property(e => e.Document1)
                .HasMaxLength(255)
                .HasColumnName("Document");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.ModDate)
                .HasColumnType("datetime")
                .HasColumnName("mod_date");
            entity.Property(e => e.ModUser)
                .HasMaxLength(50)
                .HasColumnName("mod_user");
            entity.Property(e => e.PropertyId).HasColumnName("PropertyID");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("Documents_PropertyID");
        });

        modelBuilder.Entity<ImportDataIssue>(entity =>
        {
            entity.HasIndex(e => e.DataRuleId, "DataRuleID");

            entity.HasIndex(e => e.Id, "ID");

            entity.HasIndex(e => e.IssueDate, "IssueDate");

            entity.HasIndex(e => e.PropertyId, "PropertyID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AutoProcessInstructionExecuted).HasMaxLength(255);
            entity.Property(e => e.DataRuleId).HasColumnName("DataRuleID");
            entity.Property(e => e.FromFieldValue)
                .HasMaxLength(255)
                .HasColumnName("From_FieldValue");
            entity.Property(e => e.FromImportDate)
                .HasColumnType("datetime")
                .HasColumnName("From_ImportDate");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.PropertyId).HasColumnName("PropertyID");
            entity.Property(e => e.ToFieldValue)
                .HasMaxLength(255)
                .HasColumnName("To_FieldValue");
            entity.Property(e => e.ToImportDate)
                .HasColumnType("datetime")
                .HasColumnName("To_ImportDate");

            entity.HasOne(d => d.DataRule).WithMany(p => p.ImportDataIssues)
                .HasForeignKey(d => d.DataRuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ImportDataIssues_PropertyID");
        });

        modelBuilder.Entity<ImportDataRule>(entity =>
        {
            entity.HasIndex(e => e.Id, "ID");

            entity.HasIndex(e => e.ShowOnPropertiesForm, "ShowOnPropertiesForm");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AutoProcessInstruction).HasMaxLength(255);
            entity.Property(e => e.DataSource).HasMaxLength(255);
            entity.Property(e => e.FieldName).HasMaxLength(255);
            entity.Property(e => e.IssueDecription).HasMaxLength(255);
            entity.Property(e => e.ReviewAsOf).HasColumnType("datetime");

            entity.HasOne(d => d.DataSourceNavigation).WithMany(p => p.ImportDataRules)
                .HasForeignKey(d => d.DataSource)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ImportDataRules_PropertyID");
        });

        modelBuilder.Entity<ImportLog>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("ImportLog");

            entity.HasIndex(e => e.ImportId, "ImportID");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.ImportDate).HasColumnType("datetime");
            entity.Property(e => e.ImportName).HasMaxLength(100);
            entity.Property(e => e.ImportResults).HasMaxLength(500);
            entity.Property(e => e.User).HasMaxLength(100);
        });

        modelBuilder.Entity<LihtcpubArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("LIHTCPUB_archive");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => e.ImportDateTime, "ImportDateTime");

            entity.HasIndex(e => e.ImportId, "ImportID").IsUnique();

            entity.HasIndex(e => e.CdbgIdisid, "cdbg_idisid");

            entity.HasIndex(e => e.FhaNum, "fha_num");

            entity.HasIndex(e => e.HomeIdisid, "home_idisid");

            entity.HasIndex(e => e.HudId, "hud_id");

            entity.HasIndex(e => e.MffRaId, "mff_ra_id");

            entity.HasIndex(e => e.StateId, "state_id");

            entity.HasIndex(e => e.TcapIdisid, "tcap_idisid");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.Allocamt).HasColumnName("allocamt");
            entity.Property(e => e.Basis).HasColumnName("basis");
            entity.Property(e => e.Bg2000)
                .HasMaxLength(1)
                .HasColumnName("bg2000");
            entity.Property(e => e.Bond).HasColumnName("bond");
            entity.Property(e => e.Cbsa)
                .HasMaxLength(5)
                .HasColumnName("cbsa");
            entity.Property(e => e.Cdbg).HasColumnName("cdbg");
            entity.Property(e => e.CdbgAmt).HasColumnName("cdbg_amt");
            entity.Property(e => e.CdbgIdisid).HasColumnName("cdbg_idisid");
            entity.Property(e => e.Ceilunit).HasColumnName("ceilunit");
            entity.Property(e => e.Cnty1990)
                .HasMaxLength(3)
                .HasColumnName("cnty1990");
            entity.Property(e => e.Cnty2000)
                .HasMaxLength(3)
                .HasColumnName("cnty2000");
            entity.Property(e => e.Cnty2010)
                .HasMaxLength(3)
                .HasColumnName("cnty2010");
            entity.Property(e => e.CoAdd)
                .HasMaxLength(58)
                .HasColumnName("co_add");
            entity.Property(e => e.CoCty)
                .HasMaxLength(22)
                .HasColumnName("co_cty");
            entity.Property(e => e.CoSt)
                .HasMaxLength(2)
                .HasColumnName("co_st");
            entity.Property(e => e.CoTel)
                .HasMaxLength(13)
                .HasColumnName("co_tel");
            entity.Property(e => e.CoZip)
                .HasMaxLength(10)
                .HasColumnName("co_zip");
            entity.Property(e => e.Company)
                .HasMaxLength(75)
                .HasColumnName("company");
            entity.Property(e => e.Contact)
                .HasMaxLength(52)
                .HasColumnName("contact");
            entity.Property(e => e.Cosubcur)
                .HasMaxLength(5)
                .HasColumnName("cosubcur");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Datanote)
                .HasMaxLength(500)
                .HasColumnName("datanote");
            entity.Property(e => e.Dda).HasColumnName("dda");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.Fha).HasColumnName("fha");
            entity.Property(e => e.FhaNum).HasColumnName("fha_num");
            entity.Property(e => e.Fips1990)
                .HasMaxLength(11)
                .HasColumnName("fips1990");
            entity.Property(e => e.Fips2000)
                .HasMaxLength(11)
                .HasColumnName("fips2000");
            entity.Property(e => e.Fips2010)
                .HasMaxLength(11)
                .HasColumnName("fips2010");
            entity.Property(e => e.Fips2020)
                .HasMaxLength(11)
                .HasColumnName("fips2020");
            entity.Property(e => e.Fmha514).HasColumnName("fmha_514");
            entity.Property(e => e.Fmha515).HasColumnName("fmha_515");
            entity.Property(e => e.Fmha538).HasColumnName("fmha_538");
            entity.Property(e => e.Home).HasColumnName("home");
            entity.Property(e => e.HomeAmt).HasColumnName("home_amt");
            entity.Property(e => e.HomeIdisid).HasColumnName("home_idisid");
            entity.Property(e => e.Hopevi).HasColumnName("hopevi");
            entity.Property(e => e.HpviAmt).HasColumnName("hpvi_amt");
            entity.Property(e => e.Htf).HasColumnName("htf");
            entity.Property(e => e.HtfAmt).HasColumnName("htf_amt");
            entity.Property(e => e.HudId)
                .HasMaxLength(11)
                .HasColumnName("hud_id");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.IncCeil).HasColumnName("inc_ceil");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.LiUnitr).HasColumnName("li_unitr");
            entity.Property(e => e.LiUnits).HasColumnName("li_units");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.LowCeil).HasColumnName("low_ceil");
            entity.Property(e => e.Metro).HasColumnName("metro");
            entity.Property(e => e.MffRa).HasColumnName("mff_ra");
            entity.Property(e => e.MffRaId).HasColumnName("mff_ra_id");
            entity.Property(e => e.Msa)
                .HasMaxLength(4)
                .HasColumnName("msa");
            entity.Property(e => e.N0br).HasColumnName("n_0br");
            entity.Property(e => e.N1br).HasColumnName("n_1br");
            entity.Property(e => e.N2br).HasColumnName("n_2br");
            entity.Property(e => e.N3br).HasColumnName("n_3br");
            entity.Property(e => e.N4br).HasColumnName("n_4br");
            entity.Property(e => e.NTotal).HasColumnName("n_total");
            entity.Property(e => e.NUnits).HasColumnName("n_units");
            entity.Property(e => e.NUnitsr).HasColumnName("n_unitsr");
            entity.Property(e => e.NlmReason).HasColumnName("nlm_reason");
            entity.Property(e => e.NlmSpc)
                .HasMaxLength(63)
                .HasColumnName("nlm_spc");
            entity.Property(e => e.NonProf).HasColumnName("non_prof");
            entity.Property(e => e.Nonprog).HasColumnName("nonprog");
            entity.Property(e => e.Place1990)
                .HasMaxLength(4)
                .HasColumnName("place1990");
            entity.Property(e => e.Place2000)
                .HasMaxLength(5)
                .HasColumnName("place2000");
            entity.Property(e => e.Place2010).HasColumnName("place2010");
            entity.Property(e => e.Place2020)
                .HasMaxLength(5)
                .HasColumnName("place2020");
            entity.Property(e => e.Placece)
                .HasMaxLength(4)
                .HasColumnName("placece");
            entity.Property(e => e.Placefp)
                .HasMaxLength(5)
                .HasColumnName("placefp");
            entity.Property(e => e.ProjAdd)
                .HasMaxLength(35)
                .HasColumnName("proj_add");
            entity.Property(e => e.ProjCty)
                .HasMaxLength(30)
                .HasColumnName("proj_cty");
            entity.Property(e => e.ProjSt)
                .HasMaxLength(2)
                .HasColumnName("proj_st");
            entity.Property(e => e.ProjZip)
                .HasMaxLength(5)
                .HasColumnName("proj_zip");
            entity.Property(e => e.Project)
                .HasMaxLength(69)
                .HasColumnName("project");
            entity.Property(e => e.Qct).HasColumnName("qct");
            entity.Property(e => e.Qozf).HasColumnName("qozf");
            entity.Property(e => e.QozfAmt).HasColumnName("qozf_amt");
            entity.Property(e => e.Rad).HasColumnName("rad");
            entity.Property(e => e.Rdnum514).HasColumnName("rdnum_514");
            entity.Property(e => e.Rdnum515)
                .HasMaxLength(18)
                .HasColumnName("rdnum_515");
            entity.Property(e => e.Rdnum538).HasColumnName("rdnum_538");
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.RecordStatus)
                .HasMaxLength(1)
                .HasColumnName("record_status");
            entity.Property(e => e.Reg)
                .HasMaxLength(1)
                .HasColumnName("reg");
            entity.Property(e => e.RentAssistContract).HasMaxLength(33);
            entity.Property(e => e.Rentassist).HasColumnName("rentassist");
            entity.Property(e => e.Resynd).HasColumnName("resynd");
            entity.Property(e => e.Scattered).HasColumnName("scattered");
            entity.Property(e => e.St1990)
                .HasMaxLength(2)
                .HasColumnName("st1990");
            entity.Property(e => e.St2000)
                .HasMaxLength(2)
                .HasColumnName("st2000");
            entity.Property(e => e.St2010)
                .HasMaxLength(2)
                .HasColumnName("st2010");
            entity.Property(e => e.StateId)
                .HasMaxLength(24)
                .HasColumnName("state_id");
            entity.Property(e => e.Tcap).HasColumnName("tcap");
            entity.Property(e => e.TcapAmt).HasColumnName("tcap_amt");
            entity.Property(e => e.TcapIdisid).HasColumnName("tcap_idisid");
            entity.Property(e => e.Tcep).HasColumnName("tcep");
            entity.Property(e => e.TcepAmt).HasColumnName("tcep_amt");
            entity.Property(e => e.Trct1990)
                .HasMaxLength(7)
                .HasColumnName("trct1990");
            entity.Property(e => e.Trct2000)
                .HasMaxLength(7)
                .HasColumnName("trct2000");
            entity.Property(e => e.Trct2010)
                .HasMaxLength(7)
                .HasColumnName("trct2010");
            entity.Property(e => e.TrgtDis).HasColumnName("trgt_dis");
            entity.Property(e => e.TrgtEld).HasColumnName("trgt_eld");
            entity.Property(e => e.TrgtFam).HasColumnName("trgt_fam");
            entity.Property(e => e.TrgtHml).HasColumnName("trgt_hml");
            entity.Property(e => e.TrgtOther).HasColumnName("trgt_other");
            entity.Property(e => e.TrgtPop).HasColumnName("trgt_pop");
            entity.Property(e => e.TrgtSpc)
                .HasMaxLength(41)
                .HasColumnName("trgt_spc");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");
            entity.Property(e => e.YrAlloc).HasColumnName("yr_alloc");
            entity.Property(e => e.YrPis).HasColumnName("yr_pis");
            entity.Property(e => e.Yrmisflg).HasColumnName("yrmisflg");
            entity.Property(e => e.Z).HasColumnName("z");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.LihtcpubArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("LIHTCPUB_archive_PropertyID");
        });

        modelBuilder.Entity<MassHousingBenedictArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("MassHousing_Benedict_archive");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => e.ProjId, "ProjID");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.ClosingDate)
                .HasColumnType("datetime")
                .HasColumnName("Closing Date");
            entity.Property(e => e.Curprinbal)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("curprinbal");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.FirstDueDate)
                .HasColumnType("datetime")
                .HasColumnName("First Due Date");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.LoanName)
                .HasMaxLength(255)
                .HasColumnName("Loan Name");
            entity.Property(e => e.LoanNumber)
                .HasMaxLength(255)
                .HasColumnName("Loan Number");
            entity.Property(e => e.LoanType)
                .HasMaxLength(255)
                .HasColumnName("Loan Type");
            entity.Property(e => e.Loanstat)
                .HasMaxLength(255)
                .HasColumnName("loanstat");
            entity.Property(e => e.MaturityDate)
                .HasColumnType("datetime")
                .HasColumnName("Maturity Date");
            entity.Property(e => e.Mthenddt)
                .HasColumnType("datetime")
                .HasColumnName("mthenddt");
            entity.Property(e => e.OrgprinbalCalculated)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("orgprinbal - calculated");
            entity.Property(e => e.ProjId)
                .HasMaxLength(25)
                .HasColumnName("ProjID");
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.Taxtyp)
                .HasMaxLength(255)
                .HasColumnName("taxtyp");
            entity.Property(e => e.XExportDateTime)
                .HasColumnType("datetime")
                .HasColumnName("xExportDateTime");
            entity.Property(e => e.XMatch).HasColumnName("xMatch");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.MassHousingBenedictArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("PropertiesMassHousing_Benedict_archive");
        });

        modelBuilder.Entity<MassHousingFoaArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("MassHousing_FOA_archive");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => e.ImportDateTime, "ImportDateTime");

            entity.HasIndex(e => e.ImportId, "ImportID").IsUnique();

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.CityTownNameCityTownSiteAddress)
                .HasMaxLength(255)
                .HasColumnName("City Town Name (City Town -> Site Address)");
            entity.Property(e => e.CommercialProjectMaster).HasColumnName("Commercial (Project_Master)");
            entity.Property(e => e.DebtIndicator)
                .HasMaxLength(255)
                .HasColumnName("Debt Indicator");
            entity.Property(e => e.EligibleBasisAmtProjectMaster).HasColumnName("Eligible Basis Amt (Project_Master)");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.FoaTotalUnitCount).HasColumnName("FOA Total Unit Count");
            entity.Property(e => e.HudProjectNumberProjectMaster)
                .HasMaxLength(255)
                .HasColumnName("Hud Project Number (Project_Master)");
            entity.Property(e => e.HudRiskshareProjectNumberProjectMaster)
                .HasMaxLength(255)
                .HasColumnName("Hud Riskshare Project Number(Project_Master)");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.LihtcUnitsProjectMaster).HasColumnName("LIHTC Units (Project_Master)");
            entity.Property(e => e.LowIncomeRentalUnits).HasColumnName("Low Income Rental Units");
            entity.Property(e => e.ModerateIncomeRentalUnits).HasColumnName("Moderate Income Rental Units");
            entity.Property(e => e.MortgageInsuranceCodeProjectMaster)
                .HasMaxLength(255)
                .HasColumnName("Mortgage Insurance Code (Project_Master)");
            entity.Property(e => e.MortgageInsuranceDesc)
                .HasMaxLength(255)
                .HasColumnName("Mortgage Insurance Desc");
            entity.Property(e => e.NonRevenueRentalUnits).HasColumnName("Non_revenue Rental Units");
            entity.Property(e => e.PrimaryAddressSiteAddress).HasColumnName("Primary Address (Site Address)");
            entity.Property(e => e.PrincipalProgramDescriptionPrincipalProgram)
                .HasMaxLength(255)
                .HasColumnName("Principal Program Description (Principal_Program)");
            entity.Property(e => e.PrincipalProgramProjectMaster)
                .HasMaxLength(255)
                .HasColumnName("Principal Program (Project_Master)");
            entity.Property(e => e.ProgramTypeProgramType)
                .HasMaxLength(255)
                .HasColumnName("Program Type (Program_Type)");
            entity.Property(e => e.ProjectIdProjectMaster)
                .HasMaxLength(255)
                .HasColumnName("Project Id (Project_Master)");
            entity.Property(e => e.ProjectNameProjectMaster)
                .HasMaxLength(255)
                .HasColumnName("Project Name (Project_Master)");
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.RentalHoUnits).HasColumnName("Rental & HO Units");
            entity.Property(e => e.RentalUnits0BrProjectMaster).HasColumnName("Rental Units 0 Br (Project_Master)");
            entity.Property(e => e.RentalUnits13aProjectMaster).HasColumnName("Rental Units 13a (Project_Master)");
            entity.Property(e => e.RentalUnits1BrProjectMaster).HasColumnName("Rental Units 1 Br (Project_Master)");
            entity.Property(e => e.RentalUnits236ProjectMaster).HasColumnName("Rental Units 236 (Project_Master)");
            entity.Property(e => e.RentalUnits2BrProjectMaster).HasColumnName("Rental Units 2 Br (Project_Master)");
            entity.Property(e => e.RentalUnits3BrProjectMaster).HasColumnName("Rental Units 3 Br (Project_Master)");
            entity.Property(e => e.RentalUnits4BrProjectMaster).HasColumnName("Rental Units 4 Br (Project_Master)");
            entity.Property(e => e.RentalUnits5BrProjectMaster).HasColumnName("Rental Units 5 Br (Project_Master)");
            entity.Property(e => e.RentalUnits6BrProjectMaster).HasColumnName("Rental Units 6 Br (Project_Master)");
            entity.Property(e => e.RentalUnitsElderlyRestrictedProjectMaster).HasColumnName("Rental Units Elderly Restricted (Project_Master)");
            entity.Property(e => e.RentalUnitsMktRateProjectMaster).HasColumnName("Rental Units Mkt Rate (Project_Master)");
            entity.Property(e => e.RentalUnitsMrvpBuProjectMaster).HasColumnName("Rental Units Mrvp Bu (Project_Master)");
            entity.Property(e => e.RentalUnitsMrvpPbProjectMaster).HasColumnName("Rental Units Mrvp Pb (Project_Master)");
            entity.Property(e => e.RentalUnitsOtherRestrictedProjectMaster).HasColumnName("Rental Units Other Restricted (Project_Master)");
            entity.Property(e => e.RentalUnitsRapProjectMaster).HasColumnName("Rental Units Rap (Project_Master)");
            entity.Property(e => e.RentalUnitsRsProjectMaster).HasColumnName("Rental Units Rs (Project_Master)");
            entity.Property(e => e.RentalUnitsSec8ProjectMaster).HasColumnName("Rental Units Sec8 (Project_Master)");
            entity.Property(e => e.RentalUnitsStudentProjectMaster).HasColumnName("Rental Units Student (Project_Master)");
            entity.Property(e => e.RentalUnitsUnrestrictedProjectMaster).HasColumnName("Rental Units Unrestricted (Project_Master)");
            entity.Property(e => e.Section8AdministrationType)
                .HasMaxLength(255)
                .HasColumnName("Section 8 Administration Type");
            entity.Property(e => e.Section8ContractNumberSection8Contracts)
                .HasMaxLength(255)
                .HasColumnName("Section8 Contract Number(Section8_Contracts)");
            entity.Property(e => e.SiteAddressSiteAddress)
                .HasMaxLength(255)
                .HasColumnName("Site Address (Site Address)");
            entity.Property(e => e.StateSiteAddress)
                .HasMaxLength(255)
                .HasColumnName("State (Site Address)");
            entity.Property(e => e.TotalHoUnitsProjectMaster).HasColumnName("Total HO Units (Project_Master)");
            entity.Property(e => e.TotalRentalUnitsProjectMaster).HasColumnName("Total Rental Units (Project_Master)");
            entity.Property(e => e.ZipCode1SiteAddress)
                .HasMaxLength(255)
                .HasColumnName("Zip Code 1 (Site Address)");
            entity.Property(e => e.ZipCode2SiteAddress)
                .HasMaxLength(255)
                .HasColumnName("Zip Code 2 (Site Address)");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.MassHousingFoaArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("PropertiesMassHousing_FOA_archive");
        });

        modelBuilder.Entity<MfAssistanceSec8ContractsArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("MF_Assistance_&_Sec8_Contracts_archive");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => e.ImportId, "ImportID").IsUnique();

            entity.HasIndex(e => e.ImportDateTime, "archive_date_time");

            entity.HasIndex(e => e.ContractDocTypeCode, "contract_doc_type_code");

            entity.HasIndex(e => e.ProgramTypeGroupCode, "program_type_group_code");

            entity.HasIndex(e => e.PropertyId, "property_id");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.AssistedUnitsCount).HasColumnName("assisted_units_count");
            entity.Property(e => e.ContractDocTypeCode)
                .HasMaxLength(5)
                .HasColumnName("contract_doc_type_code");
            entity.Property(e => e.ContractNumber)
                .HasMaxLength(11)
                .HasColumnName("contract_number");
            entity.Property(e => e.ContractTermMonthsQty).HasColumnName("contract_term_months_qty");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.IsAccOldInd)
                .HasMaxLength(1)
                .HasColumnName("is_acc_old_ind");
            entity.Property(e => e.IsAccPerformanceBasedInd)
                .HasMaxLength(1)
                .HasColumnName("is_acc_performance_based_ind");
            entity.Property(e => e.IsHudAdministeredInd)
                .HasMaxLength(255)
                .HasColumnName("is_hud_administered_ind");
            entity.Property(e => e.ProgramTypeGroupCode)
                .HasMaxLength(20)
                .HasColumnName("program_type_group_code");
            entity.Property(e => e.ProgramTypeGroupName)
                .HasMaxLength(20)
                .HasColumnName("program_type_group_name");
            entity.Property(e => e.ProgramTypeName)
                .HasMaxLength(20)
                .HasColumnName("program_type_name");
            entity.Property(e => e.PropertyId)
                .HasMaxLength(11)
                .HasColumnName("property_id");
            entity.Property(e => e.PropertyNameText)
                .HasMaxLength(50)
                .HasColumnName("property_name_text");
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.RentToFmrDescription)
                .HasMaxLength(255)
                .HasColumnName("rent_to_FMR_description");
            entity.Property(e => e.RentToFmrRatio).HasColumnName("rent_to_FMR_ratio");
            entity.Property(e => e.TracsCurrentExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("tracs_current_expiration_date");
            entity.Property(e => e.TracsEffectiveDate)
                .HasColumnType("datetime")
                .HasColumnName("tracs_effective_date");
            entity.Property(e => e.TracsOverallExpFiscalYear).HasColumnName("tracs_overall_exp_fiscal_year");
            entity.Property(e => e.TracsOverallExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("tracs_overall_expiration_date");
            entity.Property(e => e.TracsOverallExpireQuarter)
                .HasMaxLength(2)
                .HasColumnName("tracs_overall_expire_quarter");
            entity.Property(e => e.TracsStatusName)
                .HasMaxLength(20)
                .HasColumnName("tracs_status_name");
            entity.Property(e => e._0brCount).HasColumnName("0BR_count");
            entity.Property(e => e._0brFmr).HasColumnName("0BR_FMR");
            entity.Property(e => e._1brCount).HasColumnName("1BR_count");
            entity.Property(e => e._1brFmr).HasColumnName("1BR_FMR");
            entity.Property(e => e._2brCount).HasColumnName("2BR_count");
            entity.Property(e => e._2brFmr).HasColumnName("2BR_FMR");
            entity.Property(e => e._3brCount).HasColumnName("3BR_count");
            entity.Property(e => e._3brFmr).HasColumnName("3BR_FMR");
            entity.Property(e => e._4brCount).HasColumnName("4BR_count");
            entity.Property(e => e._4brFmr).HasColumnName("4BR_FMR");
            entity.Property(e => e._5plusBrCount).HasColumnName("5plusBR_count");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.MfAssistanceSec8ContractsArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("MF_Assistance_PropertyID");
        });

        modelBuilder.Entity<MfPropertiesWithAssistanceSec8ContractsArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("MF_Properties_with_Assistance_&_Sec8_Contracts_archive");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => new { e.ImportDateTime, e.ExpUsePropertyId }, "Import_ExpUsePropID");

            entity.HasIndex(e => e.ImportDateTime, "archive_date_time");

            entity.HasIndex(e => e.AssociatedFinancingNumber, "associated_financing_Number");

            entity.HasIndex(e => e.PropertyId, "property_id");

            entity.HasIndex(e => e.PropertyNameText, "property_name_text");

            entity.HasIndex(e => e.StateCode, "state_code");

            entity.HasIndex(e => e.Zip4Code, "zip4_code");

            entity.HasIndex(e => e.ZipCode, "zip_code");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.AddressLine1Text)
                .HasMaxLength(45)
                .HasColumnName("address_line1_text");
            entity.Property(e => e.AddressLine2Text)
                .HasMaxLength(45)
                .HasColumnName("address_line2_text");
            entity.Property(e => e.AssociatedFinancingNumber)
                .HasMaxLength(255)
                .HasColumnName("associated_financing_Number");
            entity.Property(e => e.CityNameText)
                .HasMaxLength(28)
                .HasColumnName("city_name_text");
            entity.Property(e => e.CongressionalDistrictCode)
                .HasMaxLength(15)
                .HasColumnName("congressional_district_code");
            entity.Property(e => e.CountyCode)
                .HasMaxLength(3)
                .HasColumnName("county_code");
            entity.Property(e => e.CountyNameText)
                .HasMaxLength(30)
                .HasColumnName("county_name_text");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.HubNameText)
                .HasMaxLength(30)
                .HasColumnName("hub_name_text");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.Is202811Ind)
                .HasMaxLength(1)
                .HasColumnName("is_202_811_ind");
            entity.Property(e => e.Is221d3Ind)
                .HasMaxLength(1)
                .HasColumnName("is_221d3_ind");
            entity.Property(e => e.Is221d4Ind)
                .HasMaxLength(1)
                .HasColumnName("is_221d4_ind");
            entity.Property(e => e.Is236Ind)
                .HasMaxLength(1)
                .HasColumnName("is_236_ind");
            entity.Property(e => e.IsAssistedLivingInd)
                .HasMaxLength(1)
                .HasColumnName("is_assisted_living_ind");
            entity.Property(e => e.IsBmirInd)
                .HasMaxLength(1)
                .HasColumnName("is_bmir_ind");
            entity.Property(e => e.IsBoardAndCareInd)
                .HasMaxLength(1)
                .HasColumnName("is_board_and_care_ind");
            entity.Property(e => e.IsCoInsuredInd)
                .HasMaxLength(1)
                .HasColumnName("is_co_insured_ind");
            entity.Property(e => e.IsHospitalInd)
                .HasMaxLength(1)
                .HasColumnName("is_hospital_ind");
            entity.Property(e => e.IsHudHeldInd)
                .HasMaxLength(1)
                .HasColumnName("is_hud_held_ind");
            entity.Property(e => e.IsHudOwnedInd)
                .HasMaxLength(1)
                .HasColumnName("is_hud_owned_ind");
            entity.Property(e => e.IsInsuredInd)
                .HasMaxLength(1)
                .HasColumnName("is_insured_ind");
            entity.Property(e => e.IsMipInd)
                .HasMaxLength(1)
                .HasColumnName("is_mip_ind");
            entity.Property(e => e.IsNonInsuredInd)
                .HasMaxLength(1)
                .HasColumnName("is_non_insured_ind");
            entity.Property(e => e.IsNursingHomeInd)
                .HasMaxLength(1)
                .HasColumnName("is_nursing_home_ind");
            entity.Property(e => e.IsOpportunityZoneInd)
                .HasMaxLength(1)
                .HasColumnName("is_opportunity_zone_ind");
            entity.Property(e => e.IsRefinancedInd)
                .HasMaxLength(1)
                .HasColumnName("is_refinanced_ind");
            entity.Property(e => e.IsRiskSharingInd)
                .HasMaxLength(1)
                .HasColumnName("is_risk_sharing_ind");
            entity.Property(e => e.MgmtAgentAddressLine1)
                .HasMaxLength(45)
                .HasColumnName("mgmt_agent_address_line1");
            entity.Property(e => e.MgmtAgentAddressLine2)
                .HasMaxLength(45)
                .HasColumnName("mgmt_agent_address_line2");
            entity.Property(e => e.MgmtAgentCityName)
                .HasMaxLength(28)
                .HasColumnName("mgmt_agent_city_name");
            entity.Property(e => e.MgmtAgentCompanyType)
                .HasMaxLength(20)
                .HasColumnName("mgmt_agent_company_type");
            entity.Property(e => e.MgmtAgentEmailText)
                .HasMaxLength(100)
                .HasColumnName("mgmt_agent_email_text");
            entity.Property(e => e.MgmtAgentFullName)
                .HasMaxLength(56)
                .HasColumnName("mgmt_agent_full_name");
            entity.Property(e => e.MgmtAgentIndvFirstName)
                .HasMaxLength(18)
                .HasColumnName("mgmt_agent_indv_first_name");
            entity.Property(e => e.MgmtAgentIndvLastName)
                .HasMaxLength(18)
                .HasColumnName("mgmt_agent_indv_last_name");
            entity.Property(e => e.MgmtAgentIndvMiddleName)
                .HasMaxLength(18)
                .HasColumnName("mgmt_agent_indv_middle_name");
            entity.Property(e => e.MgmtAgentIndvTitleText)
                .HasMaxLength(100)
                .HasColumnName("mgmt_agent_indv_title_text");
            entity.Property(e => e.MgmtAgentMainFaxNumber)
                .HasMaxLength(18)
                .HasColumnName("mgmt_agent_main_fax_number");
            entity.Property(e => e.MgmtAgentMainPhoneNumber)
                .HasMaxLength(25)
                .HasColumnName("mgmt_agent_main_phone_number");
            entity.Property(e => e.MgmtAgentOrgName)
                .HasMaxLength(100)
                .HasColumnName("mgmt_agent_org_name");
            entity.Property(e => e.MgmtAgentParticipantId).HasColumnName("mgmt_agent_participant_id");
            entity.Property(e => e.MgmtAgentStateCode)
                .HasMaxLength(2)
                .HasColumnName("mgmt_agent_state_code");
            entity.Property(e => e.MgmtAgentZip4Code)
                .HasMaxLength(4)
                .HasColumnName("mgmt_agent_zip4_code");
            entity.Property(e => e.MgmtAgentZipCode)
                .HasMaxLength(5)
                .HasColumnName("mgmt_agent_zip_code");
            entity.Property(e => e.MsaCode)
                .HasMaxLength(4)
                .HasColumnName("msa_code");
            entity.Property(e => e.MsaNameText)
                .HasMaxLength(45)
                .HasColumnName("msa_name_text");
            entity.Property(e => e.OwnerAddressLine1)
                .HasMaxLength(45)
                .HasColumnName("owner_address_line1");
            entity.Property(e => e.OwnerAddressLine2)
                .HasMaxLength(45)
                .HasColumnName("owner_address_line2");
            entity.Property(e => e.OwnerCityName)
                .HasMaxLength(28)
                .HasColumnName("owner_city_name");
            entity.Property(e => e.OwnerCompanyType)
                .HasMaxLength(20)
                .HasColumnName("owner_company_type");
            entity.Property(e => e.OwnerEmailText)
                .HasMaxLength(100)
                .HasColumnName("owner_email_text");
            entity.Property(e => e.OwnerIndividualFirstName)
                .HasMaxLength(18)
                .HasColumnName("owner_individual_first_name");
            entity.Property(e => e.OwnerIndividualFullName)
                .HasMaxLength(60)
                .HasColumnName("owner_individual_full_name");
            entity.Property(e => e.OwnerIndividualLastName)
                .HasMaxLength(20)
                .HasColumnName("owner_individual_last_name");
            entity.Property(e => e.OwnerIndividualMiddleName)
                .HasMaxLength(18)
                .HasColumnName("owner_individual_middle_name");
            entity.Property(e => e.OwnerIndividualTitleText)
                .HasMaxLength(100)
                .HasColumnName("owner_individual_title_text");
            entity.Property(e => e.OwnerMainFaxNumberText)
                .HasMaxLength(25)
                .HasColumnName("owner_main_fax_number_text");
            entity.Property(e => e.OwnerMainPhoneNumberText)
                .HasMaxLength(25)
                .HasColumnName("owner_main_phone_number_text");
            entity.Property(e => e.OwnerOrganizationName)
                .HasMaxLength(100)
                .HasColumnName("owner_organization_name");
            entity.Property(e => e.OwnerParticipantId).HasColumnName("owner_participant_id");
            entity.Property(e => e.OwnerStateCode)
                .HasMaxLength(2)
                .HasColumnName("owner_state_code");
            entity.Property(e => e.OwnerZip4Code)
                .HasMaxLength(4)
                .HasColumnName("owner_zip4_code");
            entity.Property(e => e.OwnerZipCode)
                .HasMaxLength(5)
                .HasColumnName("owner_zip_code");
            entity.Property(e => e.OwnershipEffectiveDate)
                .HasMaxLength(255)
                .HasColumnName("ownership_effective_date");
            entity.Property(e => e.PlacedBaseCityNameText)
                .HasMaxLength(30)
                .HasColumnName("placed_base_city_name_text");
            entity.Property(e => e.PrimaryFinancingType)
                .HasMaxLength(30)
                .HasColumnName("primary_financing_type");
            entity.Property(e => e.PropertyCategoryName)
                .HasMaxLength(60)
                .HasColumnName("property_category_name");
            entity.Property(e => e.PropertyId)
                .HasMaxLength(11)
                .HasColumnName("property_id");
            entity.Property(e => e.PropertyNameText)
                .HasMaxLength(50)
                .HasColumnName("property_name_text");
            entity.Property(e => e.PropertyPhoneNumber)
                .HasMaxLength(25)
                .HasColumnName("property_phone_number");
            entity.Property(e => e.PropertyTotalUnitCount).HasColumnName("property_total_unit_count");
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.ServicingSiteNameText)
                .HasMaxLength(40)
                .HasColumnName("servicing_site_name_text");
            entity.Property(e => e.StateCode)
                .HasMaxLength(2)
                .HasColumnName("state_code");
            entity.Property(e => e.Zip4Code)
                .HasMaxLength(4)
                .HasColumnName("zip4_code");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.MfPropertiesWithAssistanceSec8ContractsArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("MF_Properties_with_Assistance_PropertyID");
        });

        modelBuilder.Entity<MhpArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("MHP_archive");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => e.ImportDateTime, "ImportDateTime");

            entity.HasIndex(e => e.ImportId, "ImportID").IsUnique();

            entity.HasIndex(e => e.MhpId, "MHP_ID");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.Account).HasMaxLength(255);
            entity.Property(e => e.AffordableUnits).HasColumnName("Affordable Units");
            entity.Property(e => e.Community).HasMaxLength(255);
            entity.Property(e => e.DoNotModifyModifiedOn)
                .HasColumnType("datetime")
                .HasColumnName("(Do Not Modify) Modified On");
            entity.Property(e => e.DoNotModifyOpportunity)
                .HasMaxLength(255)
                .HasColumnName("(Do Not Modify) Opportunity");
            entity.Property(e => e.DoNotModifyRowChecksum)
                .HasMaxLength(255)
                .HasColumnName("(Do Not Modify) Row Checksum");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.LoanAmount).HasColumnName("Loan Amount");
            entity.Property(e => e.MhpClosingDate)
                .HasColumnType("datetime")
                .HasColumnName("MHP Closing Date");
            entity.Property(e => e.MhpId)
                .HasMaxLength(255)
                .HasColumnName("MHP_ID");
            entity.Property(e => e.MhpStatus)
                .HasMaxLength(255)
                .HasColumnName("MHP Status");
            entity.Property(e => e.ProjChar40b).HasColumnName("ProjChar_40B");
            entity.Property(e => e.ProjChar40r).HasColumnName("ProjChar_40R");
            entity.Property(e => e.ProjChar5plus5).HasColumnName("ProjChar_5plus5");
            entity.Property(e => e.ProjCharCondo).HasColumnName("ProjChar_Condo");
            entity.Property(e => e.ProjCharEquityTakeout).HasColumnName("ProjChar_EquityTakeout");
            entity.Property(e => e.ProjCharGhhprospect).HasColumnName("ProjChar_GHHProspect");
            entity.Property(e => e.ProjCharInHouseClosing).HasColumnName("ProjChar_InHouseClosing");
            entity.Property(e => e.ProjCharLongTermLoan).HasColumnName("ProjChar_LongTermLoan");
            entity.Property(e => e.ProjCharMissionCritical).HasColumnName("ProjChar_MissionCritical");
            entity.Property(e => e.ProjCharNewStabilizeAtRisk).HasColumnName("ProjChar_NewStabilizeAtRisk");
            entity.Property(e => e.ProjCharOneSource).HasColumnName("ProjChar_OneSource");
            entity.Property(e => e.ProjCharPreservation).HasColumnName("ProjChar_Preservation");
            entity.Property(e => e.ProjCharRap).HasColumnName("ProjChar_RAP");
            entity.Property(e => e.ProjCharSmallLoan).HasColumnName("ProjChar_SmallLoan");
            entity.Property(e => e.ProjCharTaxCredit).HasColumnName("ProjChar_TaxCredit");
            entity.Property(e => e.ProjectAddress)
                .HasMaxLength(255)
                .HasColumnName("Project Address");
            entity.Property(e => e.ProjectCharacteristics)
                .HasMaxLength(255)
                .HasColumnName("Project Characteristics");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(255)
                .HasColumnName("Project Name");
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.RecordType)
                .HasMaxLength(255)
                .HasColumnName("Record Type");
            entity.Property(e => e.Sros)
                .HasMaxLength(255)
                .HasColumnName("SROs");
            entity.Property(e => e.Stage).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);
            entity.Property(e => e.Unrestricted).HasMaxLength(255);
            entity.Property(e => e.X20201015TaxCredit)
                .HasMaxLength(255)
                .HasColumnName("x_2020_10_15 Tax Credit");
            entity.Property(e => e._100AmiUnits)
                .HasMaxLength(255)
                .HasColumnName("100 AMI Units");
            entity.Property(e => e._1Beds)
                .HasMaxLength(255)
                .HasColumnName("1-Beds");
            entity.Property(e => e._2Beds).HasColumnName("2-Beds");
            entity.Property(e => e._30AmiUnits).HasColumnName("30 AMI Units");
            entity.Property(e => e._3Beds).HasColumnName("3-Beds");
            entity.Property(e => e._4Beds)
                .HasMaxLength(255)
                .HasColumnName("4-Beds");
            entity.Property(e => e._50AmiUnits)
                .HasMaxLength(255)
                .HasColumnName("50 AMI Units");
            entity.Property(e => e._60AmiUnits).HasColumnName("60 AMI Units");
            entity.Property(e => e._80AmiUnits)
                .HasMaxLength(255)
                .HasColumnName("80 AMI Units");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.MhpArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("MHP_archive_PropertyID");
        });

        modelBuilder.Entity<MtgAArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("mtg_a_archive");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => e.HudProjectNumber, "HUD_PROJECT_NUMBER");

            entity.HasIndex(e => e.ImportDateTime, "ImportDateTime");

            entity.HasIndex(e => e.PremiseId, "PREMISE_ID");

            entity.HasIndex(e => e.SectionOfActCode, "SECTION_OF_ACT_CODE");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.AmortizedPrincipalBalance).HasColumnName("AMORTIZED PRINCIPAL BALANCE");
            entity.Property(e => e.CurrentPrincipalAndInterest)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("CURRENT PRINCIPAL AND INTEREST");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.FinalEndorsementDate)
                .HasColumnType("datetime")
                .HasColumnName("FINAL ENDORSEMENT DATE");
            entity.Property(e => e.FirstPaymentDate)
                .HasColumnType("datetime")
                .HasColumnName("FIRST PAYMENT DATE");
            entity.Property(e => e.HolderCity)
                .HasMaxLength(255)
                .HasColumnName("HOLDER CITY");
            entity.Property(e => e.HolderName)
                .HasMaxLength(255)
                .HasColumnName("HOLDER NAME");
            entity.Property(e => e.HolderState)
                .HasMaxLength(255)
                .HasColumnName("HOLDER STATE");
            entity.Property(e => e.HudProjectNumber)
                .HasMaxLength(255)
                .HasColumnName("HUD PROJECT NUMBER");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.InitialEndorsementDate)
                .HasColumnType("datetime")
                .HasColumnName("INITIAL ENDORSEMENT DATE");
            entity.Property(e => e.InterestRate).HasColumnName("INTEREST RATE");
            entity.Property(e => e.MaturityDate)
                .HasColumnType("datetime")
                .HasColumnName("MATURITY DATE");
            entity.Property(e => e.OriginalMortgageAmount).HasColumnName("ORIGINAL MORTGAGE AMOUNT");
            entity.Property(e => e.PremiseId)
                .HasMaxLength(255)
                .HasColumnName("PREMISE ID");
            entity.Property(e => e.PropertyCity)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY CITY");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY NAME");
            entity.Property(e => e.PropertyState)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY STATE");
            entity.Property(e => e.PropertyStreet)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY STREET");
            entity.Property(e => e.PropertyZip)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY ZIP");
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.SectionOfActCode)
                .HasMaxLength(255)
                .HasColumnName("SECTION OF ACT CODE");
            entity.Property(e => e.ServicerCity)
                .HasMaxLength(255)
                .HasColumnName("SERVICER CITY");
            entity.Property(e => e.ServicerName)
                .HasMaxLength(255)
                .HasColumnName("SERVICER NAME");
            entity.Property(e => e.ServicerState)
                .HasMaxLength(255)
                .HasColumnName("SERVICER STATE");
            entity.Property(e => e.SoaCategorySubCategory)
                .HasMaxLength(255)
                .HasColumnName("SOA CATEGORY/SUB CATEGORY");
            entity.Property(e => e.Tc)
                .HasMaxLength(255)
                .HasColumnName("TC");
            entity.Property(e => e.Te)
                .HasMaxLength(255)
                .HasColumnName("TE");
            entity.Property(e => e.TermInMonths)
                .HasMaxLength(255)
                .HasColumnName("TERM IN MONTHS");
            entity.Property(e => e.Units).HasColumnName("UNITS");
            entity.Property(e => e.XarchiveDateTime)
                .HasColumnType("datetime")
                .HasColumnName("xarchive_date_time");
            entity.Property(e => e.Ximported).HasColumnName("ximported");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.MtgAArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("mtg_a_archive_PropertyID");
        });

        modelBuilder.Entity<MtgTArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("mtg_t_archive");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => e.HudProjectNumber, "HUD_PROJECT_NUMBER");

            entity.HasIndex(e => e.ImportDateTime, "ImportDateTime");

            entity.HasIndex(e => e.PremiseId, "Premise_ID");

            entity.HasIndex(e => e.SectionOfActCode, "SECTION_OF_ACT_CODE");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.FinalEndorsementDate)
                .HasMaxLength(255)
                .HasColumnName("FINAL ENDORSEMENT DATE");
            entity.Property(e => e.FirstPaymentDate)
                .HasColumnType("datetime")
                .HasColumnName("FIRST PAYMENT DATE");
            entity.Property(e => e.HolderCity)
                .HasMaxLength(255)
                .HasColumnName("HOLDER CITY");
            entity.Property(e => e.HolderName)
                .HasMaxLength(255)
                .HasColumnName("HOLDER NAME");
            entity.Property(e => e.HolderState)
                .HasMaxLength(255)
                .HasColumnName("HOLDER STATE");
            entity.Property(e => e.HudProjectNumber)
                .HasMaxLength(255)
                .HasColumnName("HUD PROJECT NUMBER");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.InitialEndorsementDate)
                .HasColumnType("datetime")
                .HasColumnName("INITIAL ENDORSEMENT DATE");
            entity.Property(e => e.InterestRate).HasColumnName("INTEREST RATE");
            entity.Property(e => e.MaturityDate)
                .HasColumnType("datetime")
                .HasColumnName("MATURITY DATE");
            entity.Property(e => e.OriginalMortgageAmount).HasColumnName("ORIGINAL MORTGAGE AMOUNT");
            entity.Property(e => e.PremiseId)
                .HasMaxLength(255)
                .HasColumnName("Premise ID");
            entity.Property(e => e.PropertyCity)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY CITY");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY NAME");
            entity.Property(e => e.PropertyState)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY STATE");
            entity.Property(e => e.PropertyStreet)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY STREET");
            entity.Property(e => e.PropertyZip)
                .HasMaxLength(255)
                .HasColumnName("PROPERTY ZIP");
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.SectionOfActCode)
                .HasMaxLength(255)
                .HasColumnName("SECTION OF ACT CODE");
            entity.Property(e => e.ServicerCity)
                .HasMaxLength(255)
                .HasColumnName("SERVICER CITY");
            entity.Property(e => e.ServicerName)
                .HasMaxLength(255)
                .HasColumnName("SERVICER NAME");
            entity.Property(e => e.ServicerState)
                .HasMaxLength(255)
                .HasColumnName("SERVICER STATE");
            entity.Property(e => e.SoaCategorySubCategory)
                .HasMaxLength(255)
                .HasColumnName("SOA CATEGORY/Sub Category");
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Tc)
                .HasMaxLength(255)
                .HasColumnName("TC");
            entity.Property(e => e.Te)
                .HasMaxLength(255)
                .HasColumnName("TE");
            entity.Property(e => e.TermDate)
                .HasColumnType("datetime")
                .HasColumnName("TERM_DATE");
            entity.Property(e => e.TermInMonths)
                .HasMaxLength(255)
                .HasColumnName("TERM IN MONTHS");
            entity.Property(e => e.TermType)
                .HasMaxLength(255)
                .HasColumnName("TERM_TYPE");
            entity.Property(e => e.TerminationTypeDescription)
                .HasMaxLength(255)
                .HasColumnName("TERMINATION TYPE DESCRIPTION");
            entity.Property(e => e.TypeClaimNonClaim)
                .HasMaxLength(255)
                .HasColumnName("TYPE (Claim/Non Claim)");
            entity.Property(e => e.Units).HasColumnName("UNITS");
            entity.Property(e => e.XarchiveDateTime)
                .HasColumnType("datetime")
                .HasColumnName("xarchive_date_time");
            entity.Property(e => e.Ximported).HasColumnName("ximported");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.MtgTArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("mtg_t_archive_PropertyID");
        });

        modelBuilder.Entity<PreservedUnitsAging>(entity =>
        {
            entity.ToTable("PreservedUnitsAging");

            entity.HasIndex(e => e.Id, "ID");

            entity.HasIndex(e => e.PropertyId, "PropertyID");

            entity.HasIndex(e => e.UnitsAtRisk, "units_at_risk_num");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PreservedThroughDate).HasColumnType("datetime");
            entity.Property(e => e.PropertyId).HasColumnName("PropertyID");
            entity.Property(e => e.RecAddBy).HasMaxLength(50);
            entity.Property(e => e.RecAddDate).HasColumnType("datetime");
            entity.Property(e => e.RecModBy).HasMaxLength(50);
            entity.Property(e => e.RecModDate).HasColumnType("datetime");
            entity.Property(e => e.S8ExpDate)
                .HasColumnType("datetime")
                .HasColumnName("S8_ExpDate");
            entity.Property(e => e.S8Pbaunits).HasColumnName("S8_PBAUnits");

            entity.HasOne(d => d.Property).WithMany(p => p.PreservedUnitsAgings)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PropertiesPreservedUnitsAging");
        });

        modelBuilder.Entity<PropertiesExcludedFromAtRiskReport>(entity =>
        {
            entity.HasKey(e => e.PropertyId);

            entity.ToTable("PropertiesExcludedFromAtRiskReport");

            entity.HasIndex(e => e.DuplicateOfPropertyId, "DuplicateOfPropertyID");

            entity.HasIndex(e => e.AtRiskReportStatusId, "SequesteredStatusId");

            entity.Property(e => e.PropertyId)
                .ValueGeneratedNever()
                .HasColumnName("PropertyID");
            entity.Property(e => e.AddDate)
                .HasColumnType("datetime")
                .HasColumnName("add_date");
            entity.Property(e => e.AddUser)
                .HasMaxLength(50)
                .HasColumnName("add_user");
            entity.Property(e => e.DuplicateOfPropertyId).HasColumnName("DuplicateOfPropertyID");
            entity.Property(e => e.ModDate)
                .HasColumnType("datetime")
                .HasColumnName("mod_date");
            entity.Property(e => e.ModUser)
                .HasMaxLength(50)
                .HasColumnName("mod_user");
            entity.Property(e => e.StatusNote).HasMaxLength(50);

            entity.HasOne(d => d.AtRiskReportStatus).WithMany(p => p.PropertiesExcludedFromAtRiskReports)
                .HasForeignKey(d => d.AtRiskReportStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("zAtRiskReportExclusionStatusPropertiesExcludedFromAtRiskRepor");

            entity.HasOne(d => d.Property).WithOne(p => p.PropertiesExcludedFromAtRiskReport)
                .HasForeignKey<PropertiesExcludedFromAtRiskReport>(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PropertiesPropertiesExcludedFromAtRiskReport");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasIndex(e => e.HudaddDate, "HUDAddDate");

            entity.HasIndex(e => e.HudupdateDate, "HUDAddDate1");

            entity.HasIndex(e => e.Hudid, "HUDID");

            entity.HasIndex(e => e.ProjectIdMh, "MassHousing_ProjectID");

            entity.HasIndex(e => e.OldS8eurIdC, "S8EurID");

            entity.HasIndex(e => e.AssociatedFinancingNumber, "associated_financing_Number");

            entity.HasIndex(e => e.CongressionalDistrictCode, "congressional_district_code");

            entity.HasIndex(e => e.CountyCode, "county_code");

            entity.HasIndex(e => e.IsAtRisk, "isAtRisk");

            entity.HasIndex(e => e.MgmtAgentParticipantId, "mgmt_agent_participant_id");

            entity.HasIndex(e => e.MsaCode, "msa_code");

            entity.HasIndex(e => e.NonprofitStatusC, "nonprofit_status_c");

            entity.HasIndex(e => e.OwnerParticipantId, "owner_participant_id");

            entity.HasIndex(e => e.PropertyName, "property_name_text");

            entity.HasIndex(e => e.State, "state_code");

            entity.HasIndex(e => e.UnitsAtRiskNum, "units_at_risk_num");

            entity.HasIndex(e => e.Zip4, "zip4_code");

            entity.HasIndex(e => e.Zip, "zip_code");

            entity.Property(e => e.PropertyId).HasColumnName("PropertyID");
            entity.Property(e => e.AddressModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("address_modified_date");
            entity.Property(e => e.Agency).HasMaxLength(15);
            entity.Property(e => e.AgencyC)
                .HasMaxLength(255)
                .HasColumnName("Agency_c");
            entity.Property(e => e.AssociatedFinancingNumber)
                .HasMaxLength(255)
                .HasColumnName("associated_financing_Number");
            entity.Property(e => e.AssociatedFinancingNumber2C)
                .HasMaxLength(255)
                .HasColumnName("associated_financing_Number_2_c");
            entity.Property(e => e.AssociatedFinancingNumber3C)
                .HasMaxLength(255)
                .HasColumnName("associated_financing_Number_3_c");
            entity.Property(e => e.AssociatedFinancingNumber4C)
                .HasMaxLength(255)
                .HasColumnName("associated_financing_Number_4_c");
            entity.Property(e => e.City).HasMaxLength(28);
            entity.Property(e => e.CommentPreservationStatusTypeOrOtherNewAffordabilityTe)
                .HasMaxLength(255)
                .HasColumnName("Comment: Preservation Status, Type or other new Affordability Te");
            entity.Property(e => e.CommlSpaceNotUpdatedMh)
                .HasMaxLength(50)
                .HasColumnName("comml_space-not_updated_mh");
            entity.Property(e => e.CongressionalDistrictCode)
                .HasMaxLength(15)
                .HasColumnName("congressional_district_code");
            entity.Property(e => e.CountyCode)
                .HasMaxLength(3)
                .HasColumnName("county_code");
            entity.Property(e => e.CountyNameText)
                .HasMaxLength(30)
                .HasColumnName("county_name_text");
            entity.Property(e => e.EarliestTracsOverallExpDateC)
                .HasColumnType("datetime")
                .HasColumnName("earliest_tracs_overall_exp_date_c");
            entity.Property(e => e.EurCatC)
                .HasMaxLength(50)
                .HasColumnName("EUR_Cat_c");
            entity.Property(e => e.HideHuddiscrepancy).HasColumnName("HideHUDDiscrepancy");
            entity.Property(e => e.HubNameText)
                .HasMaxLength(30)
                .HasColumnName("hub_name_text");
            entity.Property(e => e.HudaddDate)
                .HasColumnType("datetime")
                .HasColumnName("HUDAddDate");
            entity.Property(e => e.Hudid).HasColumnName("HUDID");
            entity.Property(e => e.HudupdateDate)
                .HasColumnType("datetime")
                .HasColumnName("HUDUpdateDate");
            entity.Property(e => e.ImportMissingProjCodeC)
                .HasMaxLength(50)
                .HasColumnName("import_missing_proj_code_c");
            entity.Property(e => e.InsuranceTypeC)
                .HasMaxLength(10)
                .HasColumnName("InsuranceType_c");
            entity.Property(e => e.Is202811Ind)
                .HasMaxLength(1)
                .HasColumnName("is_202_811_ind");
            entity.Property(e => e.Is221d3Ind)
                .HasMaxLength(1)
                .HasColumnName("is_221d3_ind");
            entity.Property(e => e.Is221d4Ind)
                .HasMaxLength(1)
                .HasColumnName("is_221d4_ind");
            entity.Property(e => e.Is236Ind)
                .HasMaxLength(1)
                .HasColumnName("is_236_ind");
            entity.Property(e => e.IsAssistedLivingInd)
                .HasMaxLength(1)
                .HasColumnName("is_assisted_living_ind");
            entity.Property(e => e.IsAtRisk).HasColumnName("isAtRisk");
            entity.Property(e => e.IsAtRisk2).HasColumnName("isAtRisk2");
            entity.Property(e => e.IsAtRisk3).HasColumnName("isAtRisk3");
            entity.Property(e => e.IsBmirInd)
                .HasMaxLength(1)
                .HasColumnName("is_bmir_ind");
            entity.Property(e => e.IsBoardAndCareInd)
                .HasMaxLength(1)
                .HasColumnName("is_board_and_care_ind");
            entity.Property(e => e.IsCoInsuredInd)
                .HasMaxLength(1)
                .HasColumnName("is_co_insured_ind");
            entity.Property(e => e.IsHospitalInd)
                .HasMaxLength(1)
                .HasColumnName("is_hospital_ind");
            entity.Property(e => e.IsHudHeldInd)
                .HasMaxLength(1)
                .HasColumnName("is_hud_held_ind");
            entity.Property(e => e.IsHudOwnedInd)
                .HasMaxLength(1)
                .HasColumnName("is_hud_owned_ind");
            entity.Property(e => e.IsInsuredInd)
                .HasMaxLength(1)
                .HasColumnName("is_insured_ind");
            entity.Property(e => e.IsMipInd)
                .HasMaxLength(1)
                .HasColumnName("is_mip_ind");
            entity.Property(e => e.IsNonInsuredInd)
                .HasMaxLength(1)
                .HasColumnName("is_non_insured_ind");
            entity.Property(e => e.IsNursingHomeInd)
                .HasMaxLength(1)
                .HasColumnName("is_nursing_home_ind");
            // entity.Property(e => e.IsOpp IsOpportunityZoneInd)
            //     .HasMaxLength(1)
            //     .HasColumnName("is_opportunity_zone_ind");
            entity.Property(e => e.IsRefinancedInd)
                .HasMaxLength(1)
                .HasColumnName("is_refinanced_ind");
            entity.Property(e => e.IsRiskSharingInd)
                .HasMaxLength(1)
                .HasColumnName("is_risk_sharing_ind");
            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.Lng).HasColumnName("lng");
            entity.Property(e => e.LocalUseRestrictionNotes).HasMaxLength(150);
            entity.Property(e => e.MgmtAgentC).HasColumnName("mgmt_agent_c");
            entity.Property(e => e.MgmtAgentParticipantId).HasColumnName("mgmt_agent_participant_id");
            entity.Property(e => e.MortgageStatusTcAndOtherRestrictions)
                .HasMaxLength(255)
                .HasColumnName("Mortgage status/ TC and other restrictions");
            entity.Property(e => e.MsaCode)
                .HasMaxLength(4)
                .HasColumnName("msa_code");
            entity.Property(e => e.MsaNameText)
                .HasMaxLength(50)
                .HasColumnName("msa_name_text");
            entity.Property(e => e.NghdC)
                .HasMaxLength(50)
                .HasColumnName("Nghd_c");
            entity.Property(e => e.NonprofitStatusC)
                .HasMaxLength(4)
                .HasColumnName("nonprofit_status_c");
            entity.Property(e => e.OldS8eurIdC).HasColumnName("Old_S8EurID_c");
            entity.Property(e => e.OrigUnitsAssistedC).HasColumnName("orig_units_assisted_c");
            entity.Property(e => e.OtherAffordabilityRestriction)
                .HasMaxLength(255)
                .HasColumnName("other_affordability_restriction");
            entity.Property(e => e.OtherAtRiskDate)
                .HasColumnType("datetime")
                .HasColumnName("other_at_risk_date");
            entity.Property(e => e.OtherPreserved).HasColumnName("other_preserved");
            entity.Property(e => e.OwnerC).HasColumnName("owner_c");
            entity.Property(e => e.OwnerParticipantId).HasColumnName("owner_participant_id");
            entity.Property(e => e.OwnershipEffectiveDate)
                .HasColumnType("datetime")
                .HasColumnName("ownership_effective_date");
            entity.Property(e => e.PlacedBaseCityNameText)
                .HasMaxLength(30)
                .HasColumnName("placed_base_city_name_text");
            entity.Property(e => e.PrimaryFinancingType)
                .HasMaxLength(30)
                .HasColumnName("primary_financing_type");
            entity.Property(e => e.ProjectAka)
                .HasMaxLength(255)
                .HasColumnName("Project/AKA");
            entity.Property(e => e.ProjectIdLending).HasColumnName("ProjectID_lending");
            entity.Property(e => e.ProjectIdMh)
                .HasMaxLength(15)
                .HasColumnName("ProjectID_mh");
            entity.Property(e => e.PropertyCategoryName)
                .HasMaxLength(60)
                .HasColumnName("property_category_name");
            entity.Property(e => e.PropertyComments)
                .HasMaxLength(255)
                .HasColumnName("property_comments");
            entity.Property(e => e.PropertyName).HasMaxLength(50);
            entity.Property(e => e.PropertyPhoneNumber)
                .HasMaxLength(25)
                .HasColumnName("property_phone_number");
            entity.Property(e => e.PropertyTotalUnitCount).HasColumnName("property_total_unit_count");
            entity.Property(e => e.ReacDate)
                .HasColumnType("datetime")
                .HasColumnName("REAC_date");
            entity.Property(e => e.ReacScore)
                .HasMaxLength(10)
                .HasColumnName("REAC_score");
            entity.Property(e => e.RecAddBy).HasMaxLength(25);
            entity.Property(e => e.RecAddDate).HasColumnType("datetime");
            entity.Property(e => e.RecModBy).HasMaxLength(25);
            entity.Property(e => e.RecModDate).HasColumnType("datetime");
            entity.Property(e => e.RentSubsidyFormerVsNowC)
                .HasMaxLength(100)
                .HasColumnName("RentSubsidyFormerVsNow_c");
            entity.Property(e => e.RiskStatusCategoryComment)
                .HasMaxLength(255)
                .HasColumnName("Risk Status Category/Comment");
            entity.Property(e => e.S8ExpirationDateOverrideC)
                .HasColumnType("datetime")
                .HasColumnName("s8_expiration_date_override_c");
            entity.Property(e => e.S8PbaUnitsOverrideC).HasColumnName("s8_pba_units_override_c");
            entity.Property(e => e.ServicingSiteNameText)
                .HasMaxLength(40)
                .HasColumnName("servicing_site_name_text");
            entity.Property(e => e.SpecialPopulationTypeC)
                .HasMaxLength(3)
                .HasColumnName("SpecialPopulationType_c");
            entity.Property(e => e.State).HasMaxLength(2);
            entity.Property(e => e.Street).HasMaxLength(45);
            entity.Property(e => e.Street2).HasMaxLength(45);
            entity.Property(e => e.TaxCreditDeveloperC).HasColumnName("TaxCreditDeveloper_c");
            entity.Property(e => e.TitleIiVi)
                .HasMaxLength(10)
                .HasColumnName("TitleII/VI");
            entity.Property(e => e.TitleIiatRiskDate)
                .HasColumnType("datetime")
                .HasColumnName("TitleIIAtRiskDate");
            entity.Property(e => e.TotalHoUnitsMh)
                .HasMaxLength(50)
                .HasColumnName("total_HO_units_mh");
            entity.Property(e => e.Units0brC).HasColumnName("Units_0Br_c");
            entity.Property(e => e.Units13aMh)
                .HasMaxLength(50)
                .HasColumnName("units_13a_mh");
            entity.Property(e => e.Units1brC).HasColumnName("Units_1Br_c");
            entity.Property(e => e.Units236Mh)
                .HasMaxLength(50)
                .HasColumnName("units_236_mh");
            entity.Property(e => e.Units2brC).HasColumnName("Units_2Br_c");
            entity.Property(e => e.Units3brC).HasColumnName("Units_3Br_c");
            entity.Property(e => e.Units4brC).HasColumnName("Units_4Br_c");
            entity.Property(e => e.Units5brC).HasColumnName("Units_5Br_c");
            entity.Property(e => e.UnitsAssisted).HasColumnName("Units_Assisted");
            entity.Property(e => e.UnitsAtRiskNum).HasColumnName("units_at_risk_num");
            entity.Property(e => e.UnitsAtRiskNum2).HasColumnName("units_at_risk_num2");
            entity.Property(e => e.UnitsAtRiskNum3).HasColumnName("units_at_risk_num3");
            entity.Property(e => e.UnitsAtRiskStatus).HasMaxLength(5);
            entity.Property(e => e.UnitsAtRiskStatus2)
                .HasMaxLength(5)
                .HasColumnName("units_at_risk_status2");
            entity.Property(e => e.UnitsAtRiskStatus3)
                .HasMaxLength(5)
                .HasColumnName("units_at_risk_status3");
            entity.Property(e => e.UnitsElderlyC).HasColumnName("Units_Elderly_c");
            entity.Property(e => e.UnitsLostC).HasColumnName("units_lost_c");
            entity.Property(e => e.UnitsMktRateMh)
                .HasMaxLength(50)
                .HasColumnName("units_mkt_rate_mh");
            entity.Property(e => e.UnitsMrvpBackupMh)
                .HasMaxLength(50)
                .HasColumnName("units_mrvp_backup_mh");
            entity.Property(e => e.UnitsMrvpPbMh)
                .HasMaxLength(50)
                .HasColumnName("units_mrvp_PB_mh");
            entity.Property(e => e.UnitsNonElderlyC).HasColumnName("Units_NonElderly_c");
            entity.Property(e => e.UnitsRapMh)
                .HasMaxLength(50)
                .HasColumnName("units_RAP_mh");
            entity.Property(e => e.UnitsRetainedC).HasColumnName("units_retained_c");
            entity.Property(e => e.UnitsRiskShareMh)
                .HasMaxLength(15)
                .HasColumnName("units_risk_share_mh");
            entity.Property(e => e.UnitsSec8Mh)
                .HasMaxLength(50)
                .HasColumnName("units_sec8_mh");
            entity.Property(e => e.UnitsStudentMh)
                .HasMaxLength(50)
                .HasColumnName("units_student_mh");
            entity.Property(e => e.UnitsSuppMh)
                .HasMaxLength(50)
                .HasColumnName("units_SUPP_mh");
            entity.Property(e => e.UnitsUnrestrictedFamilyMh)
                .HasMaxLength(50)
                .HasColumnName("units_unrestricted_family_mh");
            entity.Property(e => e.VerifiedThroughDate).HasColumnType("datetime");
            entity.Property(e => e.Zip).HasMaxLength(5);
            entity.Property(e => e.Zip4).HasMaxLength(4);
            entity.Property(e => e._121a).HasColumnName("121A");
            entity.Property(e => e._121aAgreeDateC)
                .HasColumnType("datetime")
                .HasColumnName("121aAgreeDate_c");
            entity.Property(e => e._121aDateTerminatedC)
                .HasColumnType("datetime")
                .HasColumnName("121aDateTerminated_c");
            entity.Property(e => e._121aDurationC).HasColumnName("121aDuration_c");
            entity.Property(e => e._121aFinalYearC).HasColumnName("121aFinalYear_c");
            entity.Property(e => e._121aFirstYearC).HasColumnName("121aFirstYear_c");
            entity.Property(e => e._121aTerminatedC).HasColumnName("121aTerminated_c");
            entity.Property(e => e._40b).HasColumnName("40B");
            entity.Property(e => e._40bAgreeDateC)
                .HasColumnType("datetime")
                .HasColumnName("40bAgreeDate_c");
            entity.Property(e => e._40bDateTerminatedC)
                .HasColumnType("datetime")
                .HasColumnName("40bDateTerminated_c");
            entity.Property(e => e._40bDurationC).HasColumnName("40bDuration_c");
            entity.Property(e => e._40bFinalYearC).HasColumnName("40bFinalYear_c");
            entity.Property(e => e._40bFirstYearC).HasColumnName("40bFirstYear_c");
            entity.Property(e => e._40bPerpetuity).HasColumnName("40bPerpetuity");
            entity.Property(e => e._40bRestrictionNotesC)
                .HasMaxLength(150)
                .HasColumnName("40bRestrictionNotes_c");
            entity.Property(e => e._40bTerminatedC).HasColumnName("40bTerminated_c");
        });

        modelBuilder.Entity<Tbl40Tdocument>(entity =>
        {
            entity.ToTable("tbl40TDocuments");

            entity.HasIndex(e => e.PropertyId, "PropertyID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AffordableUnits).HasColumnName("Affordable Units");
            entity.Property(e => e.DateMailed).HasColumnType("datetime");
            entity.Property(e => e.DateReceivedatCedac)
                .HasColumnType("datetime")
                .HasColumnName("DateReceivedatCEDAC");
            entity.Property(e => e.DhcdConfirm).HasColumnName("DHCD confirm");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.NoticeType).HasMaxLength(255);
            entity.Property(e => e.ProjectAddress)
                .HasMaxLength(255)
                .HasColumnName("Project Address");
            entity.Property(e => e.ProjectCity)
                .HasMaxLength(255)
                .HasColumnName("Project City");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(255)
                .HasColumnName("Project Name");
            entity.Property(e => e.PropertyId).HasColumnName("PropertyID");
            entity.Property(e => e.TotalUnits).HasColumnName("Total Units");
        });

        modelBuilder.Entity<UsdaArchive>(entity =>
        {
            entity.HasKey(e => e.ImportId);

            entity.ToTable("USDA_archive");

            entity.HasIndex(e => e.BorrowerId, "#Borrower_ID");

            entity.HasIndex(e => e.BorrowerProjectId, "BorrowerProjectID");

            entity.HasIndex(e => e.ExpUsePropertyId, "ExpUsePropertyID");

            entity.HasIndex(e => e.ImportDateTime, "ImportDateTime");

            entity.HasIndex(e => e.ProfitTypeCode, "Profit_Type_Code");

            entity.HasIndex(e => e.ProjectId, "Project_ID");

            entity.HasIndex(e => e.ProjectMfisIdKey, "Project_Mfis_Id_Key");

            entity.HasIndex(e => e.RentalCode, "Rental_Code");

            entity.HasIndex(e => e.StateCountyFipsCode, "State_County_FIPS_Code");

            entity.HasIndex(e => e.ZipCode, "Zip_Code");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.BorrowerId)
                .HasMaxLength(255)
                .HasColumnName("#Borrower_ID");
            entity.Property(e => e.BorrowerProjectId)
                .HasMaxLength(255)
                .HasColumnName("BorrowerProjectID");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.DateOfOperation)
                .HasColumnType("datetime")
                .HasColumnName("Date_Of_Operation");
            entity.Property(e => e.DateRestrictiveClauseExpires)
                .HasColumnType("datetime")
                .HasColumnName("Date_Restrictive_Clause_Expires");
            entity.Property(e => e.DateTaxCreditExpires)
                .HasColumnType("datetime")
                .HasColumnName("Date_Tax_Credit_Expires");
            entity.Property(e => e.ExpUsePropertyId).HasColumnName("ExpUsePropertyID");
            entity.Property(e => e.ImportDateTime).HasColumnType("datetime");
            entity.Property(e => e.LaborHousingType).HasColumnName("Labor_Housing_Type");
            entity.Property(e => e.MainAddressLine1)
                .HasMaxLength(255)
                .HasColumnName("Main_Address_Line1");
            entity.Property(e => e.MainAddressLine2)
                .HasMaxLength(255)
                .HasColumnName("Main_Address_Line2");
            entity.Property(e => e.MainAddressLine3)
                .HasMaxLength(255)
                .HasColumnName("Main_Address_Line3");
            entity.Property(e => e.ManagementName)
                .HasMaxLength(255)
                .HasColumnName("Management_Name");
            entity.Property(e => e.ProfitTypeCode).HasColumnName("Profit_Type_Code");
            entity.Property(e => e.ProjectCheckDigit).HasColumnName("Project_Check_Digit");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(255)
                .HasColumnName("Project_ID");
            entity.Property(e => e.ProjectMfisIdKey)
                .HasMaxLength(9)
                .HasColumnName("Project_Mfis_Id_Key");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(255)
                .HasColumnName("Project_Name");
            entity.Property(e => e.ProjectSize).HasColumnName("Project_Size");
            entity.Property(e => e.RecModBy)
                .HasMaxLength(100)
                .HasColumnName("rec_mod_by");
            entity.Property(e => e.RecModDate)
                .HasColumnType("datetime")
                .HasColumnName("rec_mod_date");
            entity.Property(e => e.RentalAssistanceUnits).HasColumnName("Rental_Assistance_Units");
            entity.Property(e => e.RentalCode)
                .HasMaxLength(255)
                .HasColumnName("Rental_Code");
            entity.Property(e => e.RevitilizationIndicator)
                .HasMaxLength(255)
                .HasColumnName("Revitilization_Indicator");
            entity.Property(e => e.StateAbbreviation)
                .HasMaxLength(255)
                .HasColumnName("State_Abbreviation");
            entity.Property(e => e.StateCountyFipsCode).HasColumnName("State_County_FIPS_Code");
            entity.Property(e => e.TaxStatusIndicator)
                .HasMaxLength(255)
                .HasColumnName("Tax_Status_Indicator");
            entity.Property(e => e.Total1BedroomUnits).HasColumnName("Total_1_Bedroom_Units");
            entity.Property(e => e.Total2BedroomUnits).HasColumnName("Total_2_Bedroom_Units");
            entity.Property(e => e.Total3BedroomUnits).HasColumnName("Total_3_Bedroom_Units");
            entity.Property(e => e.Total4BedroomUnits).HasColumnName("Total_4_Bedroom_Units");
            entity.Property(e => e.Total5BedroomUnits).HasColumnName("Total_5_Bedroom_Units");
            entity.Property(e => e.Total6BedroomUnits).HasColumnName("Total_6_Bedroom_Units");
            entity.Property(e => e.TotalHandicappedUnits).HasColumnName("Total_Handicapped_Units");
            entity.Property(e => e.VacantUnits).HasColumnName("Vacant_Units");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(255)
                .HasColumnName("Zip_Code");

            entity.HasOne(d => d.ExpUseProperty).WithMany(p => p.UsdaArchives)
                .HasForeignKey(d => d.ExpUsePropertyId)
                .HasConstraintName("FK_USDA_archive_Properties");
        });

        modelBuilder.Entity<UspsAddress>(entity =>
        {
            entity.ToTable("USPS_Addresses");

            entity.HasIndex(e => e.Longitude, "Longitude");

            entity.HasIndex(e => e.State, "State");

            entity.HasIndex(e => e.Zip, "Zip");

            entity.HasIndex(e => e.Zip4, "Zip4");

            entity.HasIndex(e => e.AddressSource, "address_source");

            entity.HasIndex(e => new { e.AddressId, e.AddressSource }, "hud_proj_num_address_source").IsUnique();

            entity.HasIndex(e => e.AddressId, "hud_project_number");

            entity.HasIndex(e => e.NumMatches, "num_matches");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddressId)
                .HasMaxLength(20)
                .HasColumnName("AddressID");
            entity.Property(e => e.AddressInfoXml).HasColumnName("address_info_xml");
            entity.Property(e => e.AddressSource)
                .HasMaxLength(255)
                .HasColumnName("address_source");
            entity.Property(e => e.Addressee).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.ExcludeFromAddressLookup).HasColumnName("exclude_from_address_lookup");
            entity.Property(e => e.Latitude).HasMaxLength(255);
            entity.Property(e => e.Longitude).HasMaxLength(255);
            entity.Property(e => e.ModDateTime)
                .HasColumnType("datetime")
                .HasColumnName("mod_date_time");
            entity.Property(e => e.NumMatches).HasColumnName("num_matches");
            entity.Property(e => e.State).HasMaxLength(2);
            entity.Property(e => e.Street).HasMaxLength(255);
            entity.Property(e => e.Street2).HasMaxLength(255);
            entity.Property(e => e.Zip).HasMaxLength(5);
            entity.Property(e => e.Zip4).HasMaxLength(4);
        });

        modelBuilder.Entity<XS8>(entity =>
        {
            entity.HasKey(e => e.ProjectId);

            entity.ToTable("xS8");

            entity.HasIndex(e => e.SoaCode, "SOA_Code");

            entity.HasIndex(e => e.MgmtAgentId, "mgmtAgentID");

            entity.HasIndex(e => e.OwnerId, "ownerID");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Agency).HasMaxLength(255);
            entity.Property(e => e.AgntAddr2)
                .HasMaxLength(255)
                .HasColumnName("Agnt_Addr2");
            entity.Property(e => e.AgntCity)
                .HasMaxLength(255)
                .HasColumnName("Agnt_City");
            entity.Property(e => e.AgntFax)
                .HasMaxLength(255)
                .HasColumnName("Agnt_Fax");
            entity.Property(e => e.AgntPhone)
                .HasMaxLength(255)
                .HasColumnName("Agnt_Phone");
            entity.Property(e => e.AgntState)
                .HasMaxLength(255)
                .HasColumnName("Agnt State");
            entity.Property(e => e.AgntZip)
                .HasMaxLength(255)
                .HasColumnName("Agnt Zip");
            entity.Property(e => e.BringIntoProperties).HasMaxLength(50);
            entity.Property(e => e.C999).HasColumnName("#C  9/99");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.CommentPreservationStatusTypeOrOtherNewAffordabilityTe)
                .HasMaxLength(255)
                .HasColumnName("Comment: Preservation Status, Type or other new Affordability Te");
            entity.Property(e => e.ConversionStatus).HasMaxLength(15);
            entity.Property(e => e.CurrFhaInsurType)
                .HasMaxLength(255)
                .HasColumnName("Curr FHA Insur Type");
            entity.Property(e => e.CurrMortSubPba).HasColumnName("Curr Mort Sub+PBA");
            entity.Property(e => e.CurrTtlPbRentSubsidyStateHud).HasColumnName("Curr Ttl   PB Rent Subsidy  (State + HUD)");
            entity.Property(e => e.CurrentMortSubUProj).HasColumnName("Current Mort Sub U/Proj");
            entity.Property(e => e.CurrentMrvpPbU1100AccOrCurrent).HasColumnName("Current MRVP PB U  (1/1/00 ACC or current)");
            entity.Property(e => e.CurrentTtlSubsidyUProj).HasColumnName("Current Ttl Subsidy U/ Proj");
            entity.Property(e => e.CurrentUMtgSubsidyOnly).HasColumnName("Current #U Mtg Subsidy Only");
            entity.Property(e => e.CurrentUMtgSubsidyPba).HasColumnName("Current  #U Mtg Subsidy & PBA");
            entity.Property(e => e.CurrentUPbaOnly).HasColumnName("Current #U PBA only");
            entity.Property(e => e.Date121aTerminated)
                .HasMaxLength(255)
                .HasColumnName("Date 121A terminated");
            entity.Property(e => e.Devs).HasColumnName("# devs");
            entity.Property(e => e.EaEurReportCategoryEa682000)
                .HasMaxLength(255)
                .HasColumnName("EA EUR Report Category (EA 6/8/2000)");
            entity.Property(e => e.EldU).HasColumnName("Eld U");
            entity.Property(e => e.EurNpRegulatoryAgreemt)
                .HasMaxLength(255)
                .HasColumnName("EUR NP regulatory agreemt?");
            entity.Property(e => e.ExpiringUseDateOrigEurDbBackup)
                .HasColumnType("datetime")
                .HasColumnName("Expiring Use Date (Orig EUR DB) BACKUP");
            entity.Property(e => e.FamilyUnrestrictedU).HasColumnName("\"Family\" (Unrestricted) U");
            entity.Property(e => e.Fha1stMtgMaturDate)
                .HasColumnType("datetime")
                .HasColumnName("FHA 1st Mtg Matur Date");
            entity.Property(e => e.FinalMtgEndorseDateEurDbOrMhfaOrigLoanCloseDtOrPr)
                .HasMaxLength(255)
                .HasColumnName("Final Mtg Endorse Date (EUR DB) or MHFA orig loan close dt or pr");
            entity.Property(e => e.FirstHudPbContractEffectiveDateIfEarlier)
                .HasMaxLength(255)
                .HasColumnName("First HUD PB contract effective date if earlier");
            entity.Property(e => e.FirstHudPbContractTermIfConsolidated)
                .HasMaxLength(255)
                .HasColumnName("First HUD PB contract term (if consolidated)");
            entity.Property(e => e.FlexSub)
                .HasMaxLength(255)
                .HasColumnName("Flex Sub?");
            entity.Property(e => e.HdcpU).HasColumnName("Hdcp U");
            entity.Property(e => e.Hfa)
                .HasMaxLength(255)
                .HasColumnName("HFA?");
            entity.Property(e => e.HudDbId1298)
                .HasMaxLength(255)
                .HasColumnName("HUD DB ID 12/98");
            entity.Property(e => e.HudDbId93099).HasColumnName("HUD DB ID 9/30/99");
            entity.Property(e => e.HudPbContractEffectvDt999Access)
                .HasColumnType("datetime")
                .HasColumnName("HUD PB Contract Effectv Dt 9/99 (ACCESS)");
            entity.Property(e => e.HudPbContractStatus)
                .HasMaxLength(255)
                .HasColumnName("HUD PB Contract Status");
            entity.Property(e => e.HudPbContractTermMosBackup).HasColumnName("HUD PB Contract Term (mos) BACKUP");
            entity.Property(e => e.HudPbOptOutDateOrLastKnownExpirDate)
                .HasColumnType("datetime")
                .HasColumnName("HUD PB Opt Out Date (or last known expir date)");
            entity.Property(e => e.HudPbU1298).HasColumnName("HUD PB U 12/98");
            entity.Property(e => e.HudPbU93099OrCurrent).HasColumnName("HUD PB U 9/30/99 or current");
            entity.Property(e => e.HudPbaAdminByHud93099)
                .HasMaxLength(255)
                .HasColumnName("HUD PBA Admin by HUD? (9/30/99)");
            entity.Property(e => e.HudPbaRiskCategory)
                .HasMaxLength(255)
                .HasColumnName("HUD PBA Risk Category");
            entity.Property(e => e.HudPbaULost).HasColumnName("HUD PBA U lost");
            entity.Property(e => e.HudProjectBasedPbRentSubsidyProgram)
                .HasMaxLength(255)
                .HasColumnName("HUD Project Based (PB) Rent Subsidy Program");
            entity.Property(e => e.HudRentSubsidyProgDetail93099)
                .HasMaxLength(255)
                .HasColumnName("HUD Rent Subsidy Prog Detail 9/30/99");
            entity.Property(e => e.InOutRiskRun2005)
                .HasMaxLength(255)
                .HasColumnName("In/Out Risk Run 2005");
            entity.Property(e => e.LastYrTitleIi)
                .HasMaxLength(255)
                .HasColumnName("Last Yr Title II");
            entity.Property(e => e.Ldf)
                .HasMaxLength(255)
                .HasColumnName("LDF?");
            entity.Property(e => e.LocalUseRestrictions)
                .HasMaxLength(255)
                .HasColumnName("Local Use Restrictions?");
            entity.Property(e => e.MgmtAgentId).HasColumnName("mgmtAgentID");
            entity.Property(e => e.MhfaCompleteDate)
                .HasColumnType("datetime")
                .HasColumnName("MHFA \"Complete\" Date");
            entity.Property(e => e.MortSubULost13a).HasColumnName("Mort Sub U lost (13a)");
            entity.Property(e => e.MortSubULost236).HasColumnName("Mort Sub U lost (236)");
            entity.Property(e => e.MortSubULostD3Bmir).HasColumnName("Mort Sub U lost d3BMIR");
            entity.Property(e => e.MortgageStatusTcAndOtherRestrictions)
                .HasMaxLength(255)
                .HasColumnName("Mortgage status/ TC and other restrictions");
            entity.Property(e => e.MtgRiskCategory)
                .HasMaxLength(255)
                .HasColumnName("Mtg Risk Category");
            entity.Property(e => e.MtgePrepayDate)
                .HasMaxLength(255)
                .HasColumnName("Mtge Prepay Date");
            entity.Property(e => e.NetAffdblUAtRiskThru2005).HasColumnName("Net Affdbl U At Risk thru 2005");
            entity.Property(e => e.NetAffordblULostAtRiskThru2005).HasColumnName("Net Affordbl U Lost / At Risk thru 2005");
            entity.Property(e => e.Nghd).HasMaxLength(255);
            entity.Property(e => e.OrigHudOrStateMortSubU).HasColumnName("Orig HUD or State Mort Sub U");
            entity.Property(e => e.OrigHudPbaUProj).HasColumnName("Orig HUD PBA U/ Proj");
            entity.Property(e => e.OrigMktU).HasColumnName("Orig Mkt U");
            entity.Property(e => e.OrigMortSubU13a).HasColumnName("Orig Mort Sub U (13A)");
            entity.Property(e => e.OrigMortSubU236).HasColumnName("Orig Mort Sub U (236)");
            entity.Property(e => e.OrigMortSubUD3Bmir).HasColumnName("Orig Mort Sub U (d3BMIR)");
            entity.Property(e => e.OrigOwnerEurDbOrMhfaDataIfNotInEurDb)
                .HasMaxLength(255)
                .HasColumnName("Orig Owner (EUR DB or MHFA data if not in EUR DB)");
            entity.Property(e => e.OrigRhs515MortU).HasColumnName("Orig RHS 515 Mort U");
            entity.Property(e => e.OrigStatePbaU).HasColumnName("Orig State PBA U");
            entity.Property(e => e.OrigTtlSubsidyUProj).HasColumnName("Orig Ttl Subsidy U/Proj");
            entity.Property(e => e.OrigUMtgSubsidyOnly).HasColumnName("Orig #U Mtg Subsidy Only");
            entity.Property(e => e.OrigUMtgSubsidyPba).HasColumnName("Orig #U Mtg Subsidy & PBA");
            entity.Property(e => e.OrigUPbaOnly).HasColumnName("Orig #U PBA Only");
            entity.Property(e => e.OtherAffdblOrTcU).HasColumnName("Other Affdbl or TC U");
            entity.Property(e => e.OtherNps202811OrNpMhfaS8OrPres)
                .HasMaxLength(255)
                .HasColumnName("Other NPS (202, 811) or NP (MHFA s8 or PRES)?");
            entity.Property(e => e.OtherPbMtgOrTcRestrictn)
                .HasMaxLength(255)
                .HasColumnName("Other PB Mtg or TC Restrictn");
            entity.Property(e => e.OwnAddr1)
                .HasMaxLength(255)
                .HasColumnName("Own_Addr1");
            entity.Property(e => e.OwnAddr2)
                .HasMaxLength(255)
                .HasColumnName("Own_Addr2");
            entity.Property(e => e.OwnCity)
                .HasMaxLength(255)
                .HasColumnName("Own_City");
            entity.Property(e => e.OwnState)
                .HasMaxLength(255)
                .HasColumnName("Own State");
            entity.Property(e => e.OwnZip)
                .HasMaxLength(255)
                .HasColumnName("Own Zip");
            entity.Property(e => e.OwnerAddressMhic)
                .HasMaxLength(255)
                .HasColumnName("Owner Address(MHIC)");
            entity.Property(e => e.OwnerEurDb)
                .HasMaxLength(255)
                .HasColumnName("Owner (EUR DB)");
            entity.Property(e => e.OwnerId).HasColumnName("ownerID");
            entity.Property(e => e.OwnerSNamePer93099HudDbOrEurDbIfMissgFromHudO)
                .HasMaxLength(255)
                .HasColumnName("Owner's Name (per 9/30/99 HUD DB or EUR DB if missg from HUD) (o");
            entity.Property(e => e.OwnrPhone)
                .HasMaxLength(255)
                .HasColumnName("Ownr Phone");
            entity.Property(e => e.PpayOptDateOrigEurDbBackup)
                .HasColumnType("datetime")
                .HasColumnName("Ppay Opt Date (Orig EUR DB) BACKUP");
            entity.Property(e => e.PrincipalsChapaIntern799)
                .HasMaxLength(255)
                .HasColumnName("Principals (CHAPA Intern 7/99)");
            entity.Property(e => e.PrincipalsEurDb)
                .HasMaxLength(255)
                .HasColumnName("Principals (EUR DB)");
            entity.Property(e => e.PrincipalsLatestAvailFromEurDbOrChapa99Intern)
                .HasMaxLength(255)
                .HasColumnName("Principals (latest avail from EUR DB or CHAPA '99 intern)");
            entity.Property(e => e.Project).HasMaxLength(255);
            entity.Property(e => e.ProjectAka)
                .HasMaxLength(255)
                .HasColumnName("Project/AKA");
            entity.Property(e => e.PropertyMgrAddress93099HudDb)
                .HasMaxLength(255)
                .HasColumnName("Property Mgr Address (9/30/99 HUD DB)");
            entity.Property(e => e.PropertyMgrPerHudDb93099)
                .HasMaxLength(255)
                .HasColumnName("Property Mgr (per HUD DB 9/30/99)");
            entity.Property(e => e.RentFmr1298).HasColumnName("Rent FMR 12/98");
            entity.Property(e => e.RentFmrSept99).HasColumnName("Rent FMR Sept 99");
            entity.Property(e => e.RentSubsidyFormerVsNow)
                .HasMaxLength(255)
                .HasColumnName("Rent subsidy (former vs now)");
            entity.Property(e => e.RhsSubsLost).HasColumnName("RHS Subs Lost");
            entity.Property(e => e.RiskStatusCategoryComment)
                .HasMaxLength(255)
                .HasColumnName("Risk Status Category/Comment");
            entity.Property(e => e.S80br).HasColumnName("S8 0BR");
            entity.Property(e => e.S81br).HasColumnName("S8 1BR");
            entity.Property(e => e.S82br).HasColumnName("S8 2BR");
            entity.Property(e => e.S83br).HasColumnName("S8 3BR");
            entity.Property(e => e.S84br).HasColumnName("S8 4BR");
            entity.Property(e => e.S85br).HasColumnName("S8 5BR");
            entity.Property(e => e.S8Contract1298)
                .HasMaxLength(255)
                .HasColumnName("S8 Contract # (12/98)");
            entity.Property(e => e.S8Contract999)
                .HasMaxLength(255)
                .HasColumnName("S8 Contract # (9/99)");
            entity.Property(e => e.S8ExpireDateHud999Db)
                .HasColumnType("datetime")
                .HasColumnName("S8 Expire Date (HUD 9/99 DB)");
            entity.Property(e => e.S8ExpireOrEurBy12312005)
                .HasMaxLength(255)
                .HasColumnName("S8 Expire (or EUR) by 12/31/2005?");
            entity.Property(e => e.S8Type)
                .HasMaxLength(255)
                .HasColumnName("S8 type");
            entity.Property(e => e.SoaCode)
                .HasMaxLength(255)
                .HasColumnName("SOA Code");
            entity.Property(e => e.SpecialPopulatnType)
                .HasMaxLength(255)
                .HasColumnName("Special Populatn Type");
            entity.Property(e => e.StatePbaULost).HasColumnName("State PBA U lost");
            entity.Property(e => e.TcAffordabilityTerm)
                .HasMaxLength(255)
                .HasColumnName("TC Affordability Term");
            entity.Property(e => e.TcDeveloperDhcdRptsOrMhic)
                .HasMaxLength(255)
                .HasColumnName("TC Developer (DHCD rpts or MHIC)");
            entity.Property(e => e.TcNps)
                .HasMaxLength(255)
                .HasColumnName("TC NPS?");
            entity.Property(e => e.TcOwnerFirstNamePerMhic1299Db)
                .HasMaxLength(255)
                .HasColumnName("TC Owner First Name ( per MHIC 12/99 DB)");
            entity.Property(e => e.TcOwnerLastNamePerMhic1299Db)
                .HasMaxLength(255)
                .HasColumnName("TC Owner Last Name (per MHIC 12/99 DB)");
            entity.Property(e => e.TcOwnerPhonePerMhic)
                .HasMaxLength(255)
                .HasColumnName("TC Owner Phone (per MHIC)");
            entity.Property(e => e.TcU).HasColumnName("#TC u");
            entity.Property(e => e.TotalMortSubsLost).HasColumnName("Total Mort Subs Lost");
            entity.Property(e => e.TotalOrigPbaU).HasColumnName("Total Orig PBA U");
            entity.Property(e => e.TotalPbaUAtRiskThru2005).HasColumnName("Total  PBA U at risk thru 2005");
            entity.Property(e => e.TotalSubsidyUAtRiskThru2005).HasColumnName("Total Subsidy U at risk thru 2005");
            entity.Property(e => e.Ttl0br).HasColumnName("Ttl 0BR");
            entity.Property(e => e.Ttl1br).HasColumnName("Ttl 1BR");
            entity.Property(e => e.Ttl2br).HasColumnName("Ttl 2BR");
            entity.Property(e => e.Ttl3br).HasColumnName("Ttl 3BR");
            entity.Property(e => e.Ttl4br).HasColumnName("Ttl 4BR");
            entity.Property(e => e.Ttl5br).HasColumnName("Ttl 5BR");
            entity.Property(e => e.TtlHudPbaUAtRiskThru2005).HasColumnName("Ttl HUD PBA u at risk thru 2005");
            entity.Property(e => e.TtlMrvppbatRiskThru2005).HasColumnName("Ttl MRVPPBat risk thru 2005");
            entity.Property(e => e.TtlOrigMortSubU).HasColumnName("Ttl Orig Mort Sub U");
            entity.Property(e => e.TtlPbaUnitsLost).HasColumnName("Ttl PBA Units Lost");
            entity.Property(e => e.TtlSubsidyULost).HasColumnName("Ttl Subsidy U lost");
            entity.Property(e => e.TtlU).HasColumnName("Ttl U");
            entity.Property(e => e.YrTcAwarded)
                .HasMaxLength(255)
                .HasColumnName("Yr TC awarded");
            entity.Property(e => e.YrTcPis)
                .HasMaxLength(255)
                .HasColumnName("Yr TC PIS");
            entity.Property(e => e.Zip).HasMaxLength(255);
            entity.Property(e => e._121aAgreeDate)
                .HasColumnType("datetime")
                .HasColumnName("121A Agree Date");
            entity.Property(e => e._121aDurationYrs).HasColumnName("121A Duration (# yrs)");
            entity.Property(e => e._121aFinalYr).HasColumnName("121A Final Yr");
            entity.Property(e => e._121aTerminated)
                .HasMaxLength(255)
                .HasColumnName("121A Terminated?");
            entity.Property(e => e._121aYr1).HasColumnName("121A Yr 1");
            entity.Property(e => e._1298MtgHolderFha)
                .HasMaxLength(255)
                .HasColumnName("12/98 Mtg Holder-FHA");
            entity.Property(e => e._1stFhaOrigIntRate).HasColumnName("1st FHA Orig Int rate");
            entity.Property(e => e._1stHudFinanceDetail)
                .HasMaxLength(255)
                .HasColumnName("1st HUD Finance Detail");
            entity.Property(e => e._1stMhfaOrFha)
                .HasMaxLength(255)
                .HasColumnName("1st MHFA or FHA#");
            entity.Property(e => e._1stOr2ndFha)
                .HasMaxLength(255)
                .HasColumnName("1st or 2nd FHA#");
            entity.Property(e => e._1stSoaFinancingTypePerHudDb)
                .HasMaxLength(255)
                .HasColumnName("1st SOA/ Financing Type (per HUD DB)");
            entity.Property(e => e._93099ContractStatusPerHuddb)
                .HasMaxLength(255)
                .HasColumnName("9/30/99 Contract Status (per HUDDB)");
            entity.Property(e => e._93099MtgHolderFha)
                .HasMaxLength(255)
                .HasColumnName("9/30/99 Mtg Holder FHA");
        });

        modelBuilder.Entity<ZAtRiskReportExclusionStatus>(entity =>
        {
            entity.ToTable("zAtRiskReportExclusionStatus");

            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Status).HasMaxLength(255);
        });

        modelBuilder.Entity<ZCity>(entity =>
        {
            entity.HasKey(e => e.City);

            entity.ToTable("zCity");

            entity.Property(e => e.City).HasMaxLength(255);
        });

        modelBuilder.Entity<ZDocumentType>(entity =>
        {
            entity.ToTable("zDocumentTypes");

            entity.HasIndex(e => e.DocumentType, "DocumentType").IsUnique();

            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DocumentType).HasMaxLength(100);
        });

        modelBuilder.Entity<ZLogoutUser>(entity =>
        {
            entity.ToTable("zLogoutUsers");

            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<ZPreservedByProgram>(entity =>
        {
            entity.ToTable("zPreservedByProgram");

            entity.HasIndex(e => e.PreservedByProgram, "zPreservedByProgramPreservedByProgram");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PreservedByProgram).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
