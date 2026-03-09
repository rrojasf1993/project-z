import type { SortOrder } from "../../cross/enums/SortOrder";
import type { HeadCell } from "../HeadCell";

export interface EnhancedTableProps {
  numSelected: number;
  onRequestSort: (event: React.MouseEvent<unknown>, property: PropertyKey) => void;
  onSelectAllClick: (event: React.ChangeEvent<HTMLInputElement>) => void;
  order: SortOrder;
  orderBy: string;
  rowCount: number;
  headCells:Array<HeadCell>
}
