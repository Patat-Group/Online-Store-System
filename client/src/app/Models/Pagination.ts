import { Products } from "./Products";

export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
  result :Products[] |any;
}
