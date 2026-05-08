export interface BaseStateModel
{
    isLoading:boolean;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    errorData?:any;
    setIsLoading:(value:boolean)=>void;
}