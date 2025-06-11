class ApiClient {
    constructor(baseUrl) {
        this.baseUrl = baseUrl; // e.g., "api/productos "
    }
    // Método auxiliar para hacer peticiones
    request(method, endpoint = "", data = null) {
        const url = this.baseUrl + endpoint;
        const options = {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            }
        };
        if (data) {
            options.body = JSON.stringify(data);
        }
        // Usamos fetch y devolvemos una Promesa
        return fetch(url, options).then(response => {
            if (!response.ok) {
                // Si el código HTTP no es 2xx, lanzamos un error (se puede mejorar manejando 400 vs 401 etc)
                throw new Error(`Error ${response.status}: ${response.statusText}`);
            }
            // Si la respuesta tiene contenido JSON (por ejemplo, GET devuelve data, POST puede devolver objeto)
            return response.json().catch(() => null);
        });
    }
}

// Clases específicas:

class   DonacionAPI extends ApiClient {
    constructor() {
        super("api/Donaciones/");
    }
    obtenerTodos() {
        return this.request('GET'); // GET api/Donaciones/
    }
    obtenerPorId(id) {
        return this.request('GET', id.toString());
    }
    crear(donacionObj) {
        return this.request('POST', "", donacionObj);
    }
    actualizar(id, donacionObj) {
        return this.request('PUT', id.toString(), donacionObj);
    }
    eliminar(id) {
        return this.request('DELETE', id.toString());
    }
}

class BeneficiarioAPI extends ApiClient {
    constructor() {
        super("api/Beneficiarios/");
    }
    obtenerTodas() {
        return this.request('GET');
    }
    modificar(id, BeneficiariosObj) {
        return this.request('PUT', id.toString(), BeneficiariosObj);
    }
    obtenerPorId(id) {
        return this.request('GET', id.toString())
    }
    crear(BeneficiariosObj) {
        return this.request('POST', "", BeneficiariosObj);
    }
    // ... similares a ProductoAPI
    eliminar(id) {
        return this.request('DELETE', id.toString());
    }
}

class UsuarioAPI extends ApiClient {
    constructor() {
        super("api/usuario/");
    }
    registrar(usuarioObj) {
        return this.request('POST', "", usuarioObj);
    }
    // Podríamos añadir obtener info, etc., si se necesitara
}

class AuthAPI extends ApiClient {
    constructor() {
        super("api/auth/");
    }
    login(Correo, password) {
        const creds = { Correo: correo, Password: password };
        return this.request('POST', "login", creds);
    }
}
