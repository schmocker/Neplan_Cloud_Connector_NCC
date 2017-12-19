#include "mex.h"
#include "NeplanProgrammingLibrary.h"

void mexFunction(int nlhs, mxArray *plhs[], int nrhs, const mxArray *prhs[])
{
    double a, b;
    double *sum;
    
    if(nrhs != 2)
    {
        mexErrMsgIdAndTxt("bitmarker:my_sum","Two inputs required.");
    }
    
    if(!mxIsDouble(prhs[0]) || mxGetNumberOfElements(prhs[0]) != 1)
    {
        mexErrMsgIdAndTxt("bitmarker:my_sum","First argument must be a number.");
    }
    
    if(!mxIsDouble(prhs[1]) || mxGetNumberOfElements(prhs[1]) != 1)
    {
        mexErrMsgIdAndTxt("bitmarker:my_sum","Secound argument must be a number.");
    }
    
    a = mxGetScalar(prhs[0]);
    b = mxGetScalar(prhs[1]);
    
    plhs[0] = mxCreateDoubleMatrix(1, 1, mxREAL);
    
    sum = mxGetPr(plhs[0]);
    
    *sum = a + b;
}
