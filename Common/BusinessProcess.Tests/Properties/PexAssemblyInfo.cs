// <copyright file="PexAssemblyInfo.cs">Copyright ©  2017</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("BusinessProcess")]
[assembly: PexInstrumentAssembly("System.Core")]
[assembly: PexInstrumentAssembly("System.Configuration")]
[assembly: PexInstrumentAssembly("Logging")]
[assembly: PexInstrumentAssembly("GroceryCheckOut.Entity")]
[assembly: PexInstrumentAssembly("GroceryCheckOutSystem.DataAccess")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Configuration")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Logging")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "GroceryCheckOut.Entity")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "GroceryCheckOutSystem.DataAccess")]

