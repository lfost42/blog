FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /BrailleAPI
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Blog/Blog.csproj", "Blog/"]
COPY ["Blog.Data/Blog.Data.csproj", "Braille.Data/"]
RUN dotnet restore "Blog/Blog.csproj"
COPY . .
WORKDIR "/src/Blog"
RUN dotnet build "Blog.csproj" -c Release -o /BrailleAPI

FROM build AS publish
RUN dotnet publish "Blog.csproj" -c Release -o /BrailleAPI

FROM base AS final
WORKDIR /BrailleAPI
COPY --from=publish /BrailleAPI .
ENTRYPOINT ["dotnet", "Blog.Data.dll"]