
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include "face-sdk.h"
#include "sdk.h"

#pragma comment(lib, "core_sdk.lib")
#define WARNING_VALUE 73.0f

typedef struct FaceFile
{
    char *fcontent;
    int flen;
} FaceF;

int ShowReturnCode(int code);
void mgv_set_log(int level);
FaceF freadAll(char *fname);
static Engine *engine = NULL;

int main(int argc, char *argv[])
{ 
    mgv_set_log(1);
    int ret = -1;
     printf("engine before,%d\n",argc);
    ret = mgv_create_engine("", &engine); 
     printf("engine after");
   // printf("mgv_create_engine return:%s,%d\n", mgv_get_error_str(ret),argc);
    if (engine == NULL)
    {
        printf("engine error");
        return -11;
    }

    FaceF fcontent1, fcontent2;
    fcontent1 = freadAll(argv[1]);
    fcontent2 = freadAll(argv[2]);

    int featLen1 = 0;
    int featLen2 = 0;
    static unsigned char featData1[4096] = {0};
    static unsigned char featData2[4096] = {0};

    featLen1 = GetFeatureFromJpeg((unsigned char *)fcontent1.fcontent, fcontent1.flen, featData1, sizeof(featData1));
    ret=ShowReturnCode(featLen1);
    if (ret <= 0)
    {
        mgv_destroy_engine(engine);
        return ret;
    }
    featLen2 = GetFeatureFromJpeg((unsigned char *)fcontent2.fcontent, fcontent2.flen, featData2, sizeof(featData2));
    ret=ShowReturnCode(featLen2);
    if (ret <= 0)
    {
        mgv_destroy_engine(engine);
        return ret;
    }

    float score = CalcFeatureSimilarity(featData1, featLen1, featData2, featLen2);
    ret = mgv_destroy_engine(engine);
printf("%f,%d",score,ret);
//getchar();
    if (score <= 57.0f)
    {
        printf("no\n");
        return 2;
    }
    else if (score > WARNING_VALUE)
    {
        printf("yes\n");
        return 1;
    }
    else
    {
        printf("uncerntainty ");
        return 3;
    }
}

FaceF freadAll(char *fname)
{
    FaceF ret;
    ret.fcontent = "";
    ret.flen = 0;
    FILE *fp;
    int flen;

    fp = fopen(fname, "rb"); // localfile文件名
    if (fp == NULL)
    {
        printf("file %s not exist.\n", fname);
        return ret;
    }
    fseek(fp, 0L, SEEK_END); /* 定位到文件末尾 */
    flen = ftell(fp);      /* 得到文件大小 */
    char *p;
    p = (char *)malloc(flen + 1); 
    if (p == NULL)
    {
        fclose(fp);
        return ret;//        　
    }
    fseek(fp, 0L, SEEK_SET); /* 定位到文件开头 */
    fread(p, flen, 1, fp);   /* 一次性读取全部文件内容 */
    fclose(fp);
    p[flen] = 0; /* 字符串结束标志 */
    ret.fcontent = p;
    ret.flen = flen;
    return ret;
}

int ShowReturnCode(int code)
{
    switch (code)
    {
        case MGV_ERR:
            printf("interal error");
            break;
        case MGV_MALLOC_ERR:
            printf("apply for memory error");
            break;
        case MGV_IMAGE_FORMAT_ERR:
            printf("picture format error");
            break;
        case MGV_PARA_ERR:
            printf("parameter error");
            break;
        case MGV_IMAGE_OUT_OF_RANGE:
            printf("resolution ratio >1080P");
            break;
        case MGV_NO_FACE_DETECTED:
            printf("no face");
            break;
        case MGV_MULTIPLE_FACES_DETECTED:
            printf("multiple face \r\n");
            break;
        default:
            break;
    }
    return code;
}
