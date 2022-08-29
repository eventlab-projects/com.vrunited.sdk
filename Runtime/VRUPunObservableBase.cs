using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

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
                _networkView.RPC("RPC_RequestSync", RpcTarget.Others);
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
                SendNetworkData(stream, info);
            }
            else
            {
                // Network player, receive data
                ReceiveNetworkData(stream, info);
            }
        }

        protected virtual void SendNetworkData(PhotonStream stream, PhotonMessageInfo info)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation.eulerAngles.y);
        }

        protected virtual void ReceiveNetworkData(PhotonStream stream, PhotonMessageInfo info)
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = Quaternion.Euler(0, (float)stream.ReceiveNext(), 0);
        }

        #endregion

        #region RPCs

        [PunRPC]
        protected virtual void RPC_RequestSync(PhotonMessageInfo info)
        {
            if (_networkView.IsMine)
            {
                ForceSync(info.Sender);
            }
        }

        protected virtual void ForceSync(Photon.Realtime.Player sender)
        {
            _networkView.RPC("RPC_ForceSync", sender, transform.position, transform.rotation);
        }

        [PunRPC]
        protected virtual void RPC_ForceSync(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        #endregion

    }

}


