using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMERSSH {
    public enum TipTayla {
        zemlya,
        Stena_vert,
        Stena_goriz
    }

    public unsafe struct tayl {
        public UInt32 dwIDBoyca;
        public TipTayla tip;

        public tayl(UInt32 dwID, TipTayla tipok) {
            dwIDBoyca = dwID;
            tip = tipok;
        }

    }
}
