namespace SQLDataMaskingConfigurator.Infrastructure
{
    interface IRepoAssembly
    {
        #region Assembly Attribute Accessors

         string AssemblyTitle();

         string AssemblyVersion();

         string AssemblyDescription();

         string AssemblyProduct();

         string AssemblyCopyright();

         string AssemblyCompany();

        #endregion
    }
}
