namespace ConsoleApp1
{
    public enum StatusCode
    {
        UNKNOWN = -99,
        OK = 0, ENGINE_ERROR = -11, NOT_ONE_PERSON = 1, UNCERTAINTY = 99,
        MGV_ERR = -1,
        MGV_MALLOC_ERR = -2,
        MGV_IMAGE_FORMAT_ERR = -3,
        MGV_PARA_ERR = -4,
        MGV_IMAGE_OUT_OF_RANGE = -5,
        MGV_NO_FACE_DETECTED = -6,
        MGV_MULTIPLE_FACES_DETECTED = -7,
    }
}
