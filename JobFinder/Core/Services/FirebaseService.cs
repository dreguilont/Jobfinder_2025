namespace JobFinder.Core.Services
{
    using Google.Cloud.Firestore;
    using JobFinder.Core.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class FirebaseService : IFirebaseService
    {
        private readonly FirestoreDb _firestore;

        public FirebaseService(FirestoreDb firestore)
        {
            _firestore = firestore;
        }

        // 👇 Implementación de las operaciones

        public async Task<string> CrearUsuario(Usuario usuario)
        {
            DocumentReference docRef = _firestore.Collection("usuarios").Document();
            await docRef.SetAsync(usuario);
            return docRef.Id;
        }

        public async Task<Usuario> ObtenerUsuario(string id)
        {
            DocumentSnapshot snapshot = await _firestore.Collection("usuarios").Document(id).GetSnapshotAsync();
            return snapshot.Exists ? snapshot.ConvertTo<Usuario>() : null;
        }

        public async Task ActualizarUsuario(Usuario usuario)
        {
            DocumentReference docRef = _firestore.Collection("usuarios").Document(usuario.Id);
            await docRef.SetAsync(usuario, SetOptions.MergeAll);
        }

        public async Task EliminarUsuario(string id)
        {
            await _firestore.Collection("usuarios").Document(id).DeleteAsync();
        }

        public async Task<string> CrearOferta(Oferta oferta)
        {
            DocumentReference docRef = _firestore.Collection("ofertas").Document();
            await docRef.SetAsync(oferta);
            return docRef.Id;
        }

        public async Task<Oferta> ObtenerOferta(string id)
        {
            DocumentSnapshot snapshot = await _firestore.Collection("ofertas").Document(id).GetSnapshotAsync();
            return snapshot.Exists ? snapshot.ConvertTo<Oferta>() : null;
        }

        public async Task<List<Oferta>> ObtenerTodasOfertas()
        {
            QuerySnapshot snapshot = await _firestore.Collection("ofertas").GetSnapshotAsync();
            return snapshot.Documents.Select(d => d.ConvertTo<Oferta>()).ToList();
        }

        public async Task ActualizarOferta(Oferta oferta)
        {
            DocumentReference docRef = _firestore.Collection("ofertas").Document(oferta.Id);
            await docRef.SetAsync(oferta, SetOptions.MergeAll);
        }

        public async Task EliminarOferta(string id)
        {
            await _firestore.Collection("ofertas").Document(id).DeleteAsync();
        }

        public async Task AplicarAOferta(string ofertaId, string usuarioId)
        {
            DocumentReference ofertaRef = _firestore.Collection("ofertas").Document(ofertaId);
            await ofertaRef.UpdateAsync("Aplicantes", FieldValue.ArrayUnion(usuarioId));
        }

        public async Task<List<Usuario>> ObtenerAplicantes(string ofertaId)
        {
            Oferta oferta = await ObtenerOferta(ofertaId);
            if (oferta?.candidatos == null) return new List<Usuario>();

            List<Usuario> aplicantes = new List<Usuario>();
            foreach (var usuarioId in oferta.candidatos)
            {
                aplicantes.Add(await ObtenerUsuario(usuarioId));
            }
            return aplicantes;
        }
    }
}
