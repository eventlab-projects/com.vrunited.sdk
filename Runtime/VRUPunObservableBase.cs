using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

namespace VRUnited
{
    public class VRUPunObservableBase : MonoBehaviourPunCallbacks, IPunObservable
    {

        #region PROTECTED ATTRIBUTES

        protected PhotonView _networkView = null;

        #endregion

        #region CREATION AND DESTRUCTION

        protected virtual void Awake()
        {
            _networkView = GetComponent<PhotonView>();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (!_networkView.IsMine)
            {
                _networkView.RPC("RPC_RequestSynch", RpcTarget.Others);
            }
        }

        protected virtual void Start()
        {
            _networkView.observableSearch = PhotonView.ObservableSearch.Manual;

            //Transform synch
            _networkView.ObservedComponents = new List<Component>();
            _networkView.ObservedComponents.Add(this);
        }

        #endregion

        #region UPDATE

        public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation.eulerAngles.y);
            }
            else
            {
                // Network player, receive data
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = Quaternion.Euler(0, (float)stream.ReceiveNext(), 0);
            }
        }

        #endregion

        #region RPCs

        [PunRPC]
        protected virtual void RPC_RequestSynch(PhotonMessageInfo info)
        {
            if (_networkView.IsMine)
            {
                _networkView.RPC("RPC_ForceSynch", info.Sender, transform.position, transform.rotation);
            }
        }

        [PunRPC]
        protected virtual void RPC_ForceSynch(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        #endregion

    }

}


