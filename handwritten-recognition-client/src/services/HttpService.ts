export default class HttpService<S, T> {
  private _baseUrl!: string;
  constructor() {
    this._baseUrl = import.meta.env.VITE_API_BASE_API_PATH;
  }
  /**
   * DoPost
   */
  public async DoPost(urlPath: string, data: S): Promise<T> {
    const url = `${this._baseUrl}/${urlPath}`;
    let stronglyTypedResult: T = {} as T;
    try {
      const result = await fetch(url, {
        body: JSON.stringify(data),
        method: "post",
      });
      if (result.ok) {
        const jsonResponse = await result.json();
        stronglyTypedResult = JSON.parse(jsonResponse) as T;
      }
    } catch (error) {
      console.error("API Call DoPost Error", error);
    }
    return stronglyTypedResult;
  }

  public async DoGet(urlPath: string): Promise<T> {
    const url = `${this._baseUrl}/${urlPath}`;
    let stronglyTypedResult: T = {} as T;
    try {
      const result = await fetch(url, { method: "get" });
      if (result.ok) {
        const jsonResponse = await result.json();
        stronglyTypedResult = jsonResponse as T;
      }
    } catch (err) {
      console.error("API Call DoGet Error", err);
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

  /**
     * DoPostMultipartFormData
urlPath:string,data:S     */
  public async DoPostMultipartFormData(urlPath: string, data: Array<File>) {
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
