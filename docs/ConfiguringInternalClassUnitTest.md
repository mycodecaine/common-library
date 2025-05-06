# Configuring Unit Test for an Internal Class

Internal classes are primarily used for encapsulation and maintainability. However, this encapsulation prevents unit tests from accessing them by default. Private class cannot being tested in any testing framework.

## 1. Add ``InternalsVisibleTo `` in ``.csproj``
In your project's ``.csproj`` file add this

```xml

<ItemGroup>
  <AssemblyAttribute Include="System.Runtime.CompilerServices.InternalsVisibleTo">
    <_Parameter1>YourTestProjectName</_Parameter1>
  </AssemblyAttribute>
</ItemGroup>

```

Example

```xml

<ItemGroup>
    <ProjectReference Include="..\Codecaine.SportService.Domain\Codecaine.SportService.Domain.csproj" />
	  <AssemblyAttribute Include="System.Runtime.CompilerServices.InternalsVisibleTo">
		  <_Parameter1>Codecaine.Implimentation.Tests</_Parameter1>
	  </AssemblyAttribute>
  </ItemGroup>

```

## 2. Rebuild solution

Your test project will be able to access internal class