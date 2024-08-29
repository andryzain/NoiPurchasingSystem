using FastReport.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NoiPurchasingSystem.Areas.MasterData.Repository;
using NoiPurchasingSystem.Areas.Purchasing.Repositories;
using NoiPurchasingSystem.Data;
using NoiPurchasingSystem.Models;
using NoiPurchasingSystem.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    //options.Password.RequiredLength = 5;
    //options.Password.RequiredUniqueChars = 1;
    options.Password.RequireNonAlphanumeric = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSession();

//Script Auto Show Login Account First Time
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddMvc(options =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddXmlSerializerFormatters().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

AddScope();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaims>();

#region Areas Master Data
builder.Services.AddScoped<IPenggunaRepository, IPenggunaRepository>();
builder.Services.AddScoped<ILevelUserRepository, ILevelUserRepository>();
builder.Services.AddScoped<IPrincipalRepository, IPrincipalRepository>();
builder.Services.AddScoped<ISatuanRepository, ISatuanRepository>();
builder.Services.AddScoped<IKategoriRepository, IKategoriRepository>();
builder.Services.AddScoped<IDiskonRepository, IDiskonRepository>();
builder.Services.AddScoped<IMetodePembayaranRepository, IMetodePembayaranRepository>();
builder.Services.AddScoped<IBengkelRepository, IBengkelRepository>();
builder.Services.AddScoped<IMekanikRepository, IMekanikRepository>();
builder.Services.AddScoped<IProdukRepository, IProdukRepository>();
builder.Services.AddScoped<IBankRepository, IBankRepository>();
builder.Services.AddScoped<IBeratRepository, IBeratRepository>();
builder.Services.AddScoped<IItemReimbursmentRepository, IItemReimbursmentRepository>();
#endregion

//#region Areas Penerimaan
//builder.Services.AddScoped<IReceiveOrderRepository, IReceiveOrderRepository>();
//#endregion

#region Areas Purchasing
builder.Services.AddScoped<IPurchaseRequestRepository, IPurchaseRequestRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, IPurchaseOrderRepository>();
//builder.Services.AddScoped<IPembayaranBarangRepository, IPembayaranBarangRepository>();
#endregion

//#region Areas Pengiriman
//builder.Services.AddScoped<IDeliveryOrderRepository, IDeliveryOrderRepository>();
//#endregion

//#region Areas Transaksi
//builder.Services.AddScoped<IPurchaseRequestRepository, IPurchaseRequestRepository>();
//builder.Services.AddScoped<IApprovalPurchaseRequestRepository, IApprovalPurchaseRequestRepository>();
//builder.Services.AddScoped<IPurchaseOrderRepository, IPurchaseOrderRepository>();
//#endregion

//#region Areas Administrasi
//builder.Services.AddScoped<IInvoiceRepository, IInvoiceRepository>();
//builder.Services.AddScoped<IPembayaranRepository, IPembayaranRepository>();
//#endregion

//#region Areas Keuangan
//builder.Services.AddScoped<IPiutangUsahaRepository, IPiutangUsahaRepository>();
//builder.Services.AddScoped<IHutangUsahaRepository, IHutangUsahaRepository>();
//#endregion

//#region Areas Reimbursment
//builder.Services.AddScoped<IPengajuanRepository, IPengajuanRepository>();
//builder.Services.AddScoped<IPersetujuanRepository, IPersetujuanRepository>();
//builder.Services.AddScoped<IPembayaranReimbursmentRepository, IPembayaranReimbursmentRepository>();
//#endregion

//Initialize Fast Report
FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.UseFastReport();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();

    endpoints.MapControllerRoute(
        name: "MyArea",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
});

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] {
        #region Area Master Data Menu Role Pengguna
        "Role", "IndexRole", "CreateRole", "DetailRole", "DeleteRole",
        #endregion
    //    #region Area Master Data Menu level Pengguna
    //    "LevelPengguna", "IndexLevelPengguna", "CreateLevelPengguna", "DetailLevelPengguna", "DeleteLevelPengguna",
    //    #endregion
    //    #region Area Master Data Menu Pengguna
    //    "Pengguna", "IndexPengguna", "CreatePengguna", "DetailPengguna", "DeletePengguna",
    //    #endregion
    //    #region Area Master Data Menu Principal
    //    "Principal", "IndexPrincipal", "CreatePrincipal", "DetailPrincipal", "DeletePrincipal",
    //    #endregion
    //    #region Area Master Data Menu Diskon Produk
    //    "Diskon", "IndexDiskon", "CreateDiskon", "DetailDiskon", "DeleteDiskon",
    //    #endregion
    //    #region Area Master Data Menu Satuan Produk
    //    "Satuan", "IndexSatuan", "CreateSatuan", "DetailSatuan", "DeleteSatuan",
    //    #endregion
    //    #region Area Master Data Menu Kategori Produk
    //    "Kategori", "IndexKategori", "CreateKategori", "DetailKategori", "DeleteKategori",
    //    #endregion
    //    #region Area Master Data Menu Produk
    //    "Produk", "IndexProduk", "CreateProduk", "DetailProduk", "DeleteProduk",
    //    #endregion
    //    #region Area Master Data Menu Metode Pembayaran
    //    "MetodePembayaran", "IndexMetodePembayaran", "CreateMetodePembayaran", "DetailMetodePembayaran", "DeleteMetodePembayaran",
    //    #endregion
    //    #region Area Master Data Menu Bengkel
    //    "Bengkel", "IndexBengkel", "CreateBengkel", "DetailBengkel", "DeleteBengkel",
    //    #endregion
    //    #region Area Master Data Menu Mekanik
    //    "Mekanik", "IndexMekanik", "CreateMekanik", "DetailMekanik", "DeleteMekanik",
    //    #endregion
    //    #region Area Master Data Menu Bank
    //    "Bank", "IndexBank", "CreateBank", "DetailBank", "DeleteBank",
    //    #endregion
    //    #region Area Master Data Menu Bank Cabang
    //    "BankCabang", "IndexBankCabang", "CreateBankCabang", "DetailBankCabang", "DeleteBankCabang",
    //    #endregion
    //    #region Area Master Data Menu Item Reimbursment
    //    "ItemReimbursment", "IndexItemReimbursment", "CreateItemReimbursment", "DetailItemReimbursment", "DeleteItemReimbursment",
    //    #endregion

    //    #region Area Penerimaan Menu Receive Order
    //    "ReceiveOrder", "IndexReceiveOrder", "CreateReceiveOrder", "DetailReceiveOrder",
    //    #endregion       

    //    #region Area Pembelian Menu Permintaan
    //    "Permintaan", "IndexPermintaan", "CreatePermintaan", "DetailPermintaan", "DeletePermintaan", "GeneratePembelian-Permintaan",
    //    #endregion       

    //    #region Area Pembelian Menu Pembelian
    //    "Pembelian", "IndexPembelian", "CreatePembelian", "DetailPembelian", "GeneratePayment-Pembelian",
    //    #endregion

    //    #region Area Pembelian Menu Pembayaran Barang
    //    "PembayaranBarang", "IndexPembayaranBarang", "DetailPembayaranBarang",
    //    #endregion

    //    #region Area Pengiriman Menu Pengiriman Barang
    //    "DeliveryOrder", "IndexDeliveryOrder", "CreateDeliveryOrder", "DetailDeliveryOrder",
    //    #endregion

    //    #region Area Transaksi Menu Purchase Request
    //    "PurchaseRequest", "IndexPurchaseRequest", "CreatePurchaseRequest", "DetailPurchaseRequest", "DeletePurchaseRequest", "GeneratePurchaseOrder-PurchaseRequest",
    //    #endregion

    //    #region Area Transaksi Menu Approval Purchase Request
    //    "ApprovalPurchaseRequest", "IndexApprovalPurchaseRequest", "DetailApprovalPurchaseRequest",
    //    #endregion

    //    #region Area Transaksi Menu Purchase Order
    //    "PurchaseOrder", "IndexPurchaseOrder", "DetailPurchaseOrder", "GenerateDeliveryOrder-PurchaseOrder",
    //    #endregion

    //    #region Area Administrasi Menu Invoice
    //    "Invoice", "IndexInvoice", "DetailInvoice", "GeneratePayment-Invoice",
    //    #endregion

    //    #region Area Administrasi Menu Pembayaran
    //    "Pembayaran", "IndexPembayaran", "DetailPembayaran",
    //    #endregion

    //    #region Area Keuangan Menu Piutang Usaha
    //    "PiutangUsaha", "IndexPiutangUsaha", "DetailPiutangUsaha",
    //    #endregion

    //    #region Area Keuangan Menu Hutang Usaha
    //    "HutangUsaha", "IndexHutangUsaha", "DetailHutangUsaha",
    //    #endregion

    //    #region Area Reimbursment Menu Pengajuan
    //    "Pengajuan", "IndexPengajuan", "CreatePengajuan", "DetailPengajuan", "DeletePengajuan",
    //    #endregion

    //    #region Area Reimbursment Menu Persetujuan
    //    "Persetujuan", "IndexPersetujuan", "DetailPersetujuan", "GeneratePayment-Persetujuan",
    //    #endregion

    //    #region Area Reimbursment Menu Pembayaran Reimbursment
    //    "PembayaranReimbursment", "IndexPembayaranReimbursment", "DetailPembayaranReimbursment",
    //    #endregion
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

app.Run();

void AddScope()
{
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
}