async function handleDotnetReady() {
    const exports = await Module.getAssemblyExports("SwissPension.WasmPrototype.Frontend");
    // Bind all exported functions to a global $
    window.$ = exports.SwissPension.WasmPrototype.Frontend.Program;
    console.log("Dotnet WASM ready, exports available on $");
}
