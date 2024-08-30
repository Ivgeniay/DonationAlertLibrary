using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAlertsApi.Models.Data
{
    public class Locales
    {
        public Dictionary<LocalesType, string> localesData = new()
        {
            { LocalesType.be_BY, "Belarusian" },
            { LocalesType.de_DE, "German" },
            { LocalesType.en_US, "English (USA)" },
            { LocalesType.es_ES, "Spanish" },
            { LocalesType.es_US, "Spanish (USA)" },
            { LocalesType.et_EE, "Estonian" },
            { LocalesType.fr_FR, "French" },
            { LocalesType.he_HE, "Hebrew" },
            { LocalesType.it_IT, "Italian" },
            { LocalesType.ka_GE, "Georgian" },
            { LocalesType.kk_KZ, "Kazakh" },
            { LocalesType.ko_KR, "Korean" },
            { LocalesType.lv_LV, "Latvian" },
            { LocalesType.pl_PL, "Polish" },
            { LocalesType.pt_BR, "Portuguese (Brazil)" },
            { LocalesType.ru_RU, "Russian" },
            { LocalesType.sv_SE, "Swedish" },
            { LocalesType.tr_TR, "Turkish" },
            { LocalesType.uk_UA, "Ukrainian" },
            { LocalesType.zh_CN, "Chinese" }, 
        };

        public string GetLocale(LocalesType locale)
        {
            return localesData[locale];
        }
    }

    public enum LocalesType
    {
        be_BY,
        de_DE,
        en_US,
        es_ES,
        es_US,
        et_EE,
        fr_FR,
        he_HE,
        it_IT,
        ka_GE,
        kk_KZ,
        ko_KR,
        lv_LV,
        pl_PL,
        pt_BR,
        ru_RU,
        sv_SE,
        tr_TR,
        uk_UA,
        zh_CN,
    }
}
