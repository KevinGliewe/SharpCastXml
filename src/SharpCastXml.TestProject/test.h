#ifndef TEST_H
#define TEST_H

#ifdef __castxml__
//#define JSONDICT(data) "{"#data"}"
//#define ANNOTATE(data) __attribute__((annotate (JSONDICT(data))))
#define ANNOTATE(data) __attribute__((annotate (data)))
#else
#define ANNOTATE(data) /*data*/
#endif

typedef struct { float x; float y; float z; } float3;
typedef unsigned int uint;

#endif