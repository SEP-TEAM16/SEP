namespace SEP.Gateway.DTO
{
    public class MicroservicesDTO
    {
        public GlobalConfigurationDTO GlobalConfiguration { get; set; }
        public List<RouteDTO> Routes { get; set; }
        public MicroservicesDTO() { }
        public MicroservicesDTO(GlobalConfigurationDTO globalConfiguration, List<RouteDTO> routes)
        {
            GlobalConfiguration = globalConfiguration;
            Routes = routes;
        }
    }
}
