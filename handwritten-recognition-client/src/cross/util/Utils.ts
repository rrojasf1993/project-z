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
}
