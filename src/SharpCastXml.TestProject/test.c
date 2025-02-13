
//#include "test.h"

namespace TestNs1::TestNs2 {
    namespace TestNs3 {

        struct ANNOTATE("view:'Test',hello:'world'")
            STest2
        {
            ANNOTATE("an_anotation")
            int m_iTest;
            float m_fTest;
            float3 m_vTest;
            uint m_aTest[ARRSIZE];

            struct STest3 { int a; };
        };
    }
}