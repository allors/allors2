export interface SideMenuItem {
    icon?: string;
    title?: string;
    link?: string;
    children?: SideMenuItem[];
    open?: boolean;
    id?: string;
}
