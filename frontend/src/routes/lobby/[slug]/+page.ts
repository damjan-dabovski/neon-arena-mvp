import type { Load } from "@sveltejs/kit";
import type { RouteLoadParameter } from "../../../types";

export const load: Load = ({ params }): RouteLoadParameter => {
    return {
        slug: params.slug ?? ''
    };
};
