let setErrorFunc: ((msg: string) => void) | null = null;

export function registerErrorSetter(fn: (msg: string) => void) {
    setErrorFunc = fn;
}

export function showError(msg: string) {
    if (setErrorFunc) {
        setErrorFunc(msg);
        //if (color == "red") {
        //    setErrorFunc(msg);
        //}
        // else {
        //    setErrorFunc(msg + " groen");
        //}
            

        // hoelang hij blijft staan 1000 = 1sec
        setTimeout(() => setErrorFunc!(""), 5000);
    }
}
