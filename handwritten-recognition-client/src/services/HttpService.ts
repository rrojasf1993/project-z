export default class HttpService<S, T> {
  private _baseUrl!: string;
  constructor() {
    this._baseUrl = import.meta.env.VITE_API_BASE_API_PATH;
  }
  /**
   * DoBaseHttpRequest
   */
  public async DoBaseHttpRequest(
    urlPath: string,
    method: "get" | "post" | "patch" | "put",
    data?: S,
  ): Promise<T> {
    const url = `${this._baseUrl}/${urlPath}`;
    let stronglyTypedResult: T = {} as T;
    let result: Response = {} as Response;
    try {
      if (method !== "get") {
        result = await fetch(url, {
          body: JSON.stringify(data),
          method: method,
          headers:{
            "content-type":"application/json"
          }
        });
      } else {
        result = await fetch(url, {
          method: method,
        });
      }
      if (result.ok) {
        const jsonResponse = await result.json();
        stronglyTypedResult =
          method !== "get"
            ? (JSON.parse(jsonResponse) as T)
            : (jsonResponse as T);
      }
    } catch (error) {
      console.error("API Call DoBaseHttpRequest Error", error);
    }
    return stronglyTypedResult;
  }

  public async DownloadFile(urlPath: string): Promise<Blob> {
    const url = `${this._baseUrl}/${urlPath}`;
    let data = {} as Blob;
    try {
      const result = await fetch(url, { method: "get" });
      if (result.ok) {
        data = await result.blob();
      }
    } catch (err) {
      console.error("API Call Download Error", err);
    }
    return data;
  }

  public async DoPostMultipartFormData(
    urlPath: string,
    data: Array<File>,
  ): Promise<T> {
    const url = `${this._baseUrl}/${urlPath}`;
    let stronglyTypedResult: T = {} as T;
    const formData = new FormData();
    data.forEach(async (fileInfo: File) => {
      const realFile: File = fileInfo;
      try {
        formData.append(realFile.name, realFile);
      } catch (error) {
        console.error(`Failed to load file from: ${realFile.name}`, error);
        return;
      }
    });
    try {
      const response = await fetch(url, {
        method: "POST",
        body: formData,
        headers: {
          "Content-Disposition": "form-data",
          name: "files",
        },
      });
      if (!response.ok) throw new Error("Network response was not ok");
      console.log("Files uploaded successfully:");
      stronglyTypedResult = (await response.json()) as T;
    } catch (error) {
      console.error("Failed to upload files", error);
    }
    return stronglyTypedResult;
  }
}
