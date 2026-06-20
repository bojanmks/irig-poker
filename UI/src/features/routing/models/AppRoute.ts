export type AppRoute = {
    basePath?: string;
    path?: string;
    layoutElement?: React.JSX.Element;
    element?: React.JSX.Element;
    children?: AppRoute[];
};