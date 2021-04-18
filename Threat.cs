using System;
using System.Collections.Generic;
using System.Text;

namespace LABA2
{
    public class Threat
    {
        public string ThreatID { get; set; }
        public string ThreatName { get; set; }
        public string ThreatDescription { get; set; }
        public string ThreatSource { get; set; }
        public string ThreatObject { get; set; }
        public string ThreatConf { get; set; }
        public string ThreatInteg { get; set; }
        public string ThreatAccess { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Threat threat &&
                   ThreatID == threat.ThreatID &&
                   ThreatName == threat.ThreatName &&
                   ThreatDescription == threat.ThreatDescription &&
                   ThreatSource == threat.ThreatSource &&
                   ThreatObject == threat.ThreatObject &&
                   ThreatConf == threat.ThreatConf &&
                   ThreatInteg == threat.ThreatInteg &&
                   ThreatAccess == threat.ThreatAccess;
        }
    }
}
