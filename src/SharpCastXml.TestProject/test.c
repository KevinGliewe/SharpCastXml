
//#include "test.h"

struct ANNOTATE("test:true,other:yes")
	STest
{
	ANNOTATE("an_anotation")
	int m_iTest;
	float m_fTest;
	float3 m_vTest;
	uint m_aTest[ARRSIZE];
};