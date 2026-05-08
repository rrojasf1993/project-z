import type { SortOrder } from "../enums/SortOrder";

export class Utils {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public descendingComparator(a: any, b: any, orderBy: PropertyKey): any {
    if (b[orderBy] < a[orderBy]) {
      return -1;
    }
    if (b[orderBy] > a[orderBy]) {
      return 1;
    }
    return 0;
  }

  public getComparator<Key extends PropertyKey>(
    order: SortOrder,
    orderBy: Key,
  ): (
    a: { [key in Key]: number | string },
    b: { [key in Key]: number | string },
  ) => number {
    return order === "desc"
      ? (a, b) => this.descendingComparator(a, b, orderBy)
      : (a, b) => -this.descendingComparator(a, b, orderBy);
  }

  public async readPngFromStream(stream: ReadableStream) {
    if (!stream || !(stream instanceof ReadableStream)) {
      throw new Error("Invalid stream");
    }

    const reader = stream.getReader();

    try {
      while (true) {
        const { done, value } = await reader.read();
        if (done) {
          break;
        }
        // Assuming value is a Uint8Array representing the PNG image data
        const pngData = new Uint8Array(value);

        // Do something with pngData, like loading it into an Image object
        const img = new Image();
        img.src =
          "data:image/png;base64," + btoa(String.fromCharCode(...pngData));
        return img;
      }
      // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-explicit-any
    } catch (error: any) {
      throw new Error("Failed to read PNG image from stream");
    }
  }
}
