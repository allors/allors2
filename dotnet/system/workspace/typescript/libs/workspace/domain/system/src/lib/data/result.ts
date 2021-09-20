import { Select } from "./Select";

export interface Result {
  selectRef?: string;

  select?: Select;

  name?: string;

  skip?: number;

  take?: number;
}
