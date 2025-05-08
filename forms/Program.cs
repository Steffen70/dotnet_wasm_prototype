using System.Windows.Forms;
using SwissPension.WasmPrototype.Forms;
using Syncfusion.Licensing;

SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXpedXVVR2VZUkR3X0VWYE4=");

ApplicationConfiguration.Initialize();
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

Application.Run(new MainForm());