using SQLDataMaskingConfigurator.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SQLDataMaskingConfigurator.Services
{
    class FileIOService
    {
        readonly Logger Logger;
        readonly ConfigHelper ConfigHelper;
        readonly int BufferSize = 80 * 1024;

        public FileIOService(Logger _Logger, ConfigHelper _ConfigHelper)
        {
            this.Logger = _Logger;
            this.ConfigHelper = _ConfigHelper;
        }

        #region FileIO Public Methods

        public bool MoveFile(string _OldFileName, string _DestFileName)
        {
            bool val = false;
            try
            {
                _ = DeleteFile(_DestFileName);
                if (File.Exists(_OldFileName))
                {
                    File.Move(_OldFileName, _DestFileName);
                }
                val = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "MoveFile");
            }

            return val;
        }

        public bool DeleteFile(string _DestFileName)
        {
            bool val = false;
            try
            {
                if (File.Exists(_DestFileName))
                {
                    File.Delete(_DestFileName);
                }
                val = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "MoveFile");
            }

            return val;
        }

        public bool CopyFile(string _InPath, string _OutPath)
        {
            bool val = false;
            try
            {
                using (var ins = new FileStream(_InPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    using (var ops = new FileStream(_OutPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        val = CopyStream(ins, ops);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyFile");
            }

            return val;
        }

        public string ReadFileContentAsString(string _FileNameWithPath)
        {
            if (File.Exists(_FileNameWithPath))
            {
                return File.ReadAllText(_FileNameWithPath);
            }
            else { return string.Empty; }
        }

        public string ResolveFileNameEndingChar(char _EndingChar, string _FileName)
        {
            string fName = Path.GetFileNameWithoutExtension(_FileName);
            string ext = Path.GetExtension(_FileName);
            if (fName.EndsWith(_EndingChar.ToString()))
            {
                fName = fName.Remove(fName.Length - 1, 1);
                //fName = fName.Replace(_EndingChar.ToString(), string.Empty);
            }
            return fName + ext;
        }

        public List<string> GetAllFilesFromDirectory(string _Path, string[] _SearchPatterns, SearchOption searchOption = SearchOption.TopDirectoryOnly, bool IgnoreSearchPattern = false)
        {
            if (!IgnoreSearchPattern)
            {
                return _SearchPatterns.AsParallel()
                       .SelectMany(searchPattern =>
                              Directory.EnumerateFiles(_Path, searchPattern, searchOption)).ToList();
            }
            else
            {
                return _SearchPatterns.AsParallel()
                      .SelectMany(searchPattern =>
                             Directory.EnumerateFiles(_Path)).Distinct().ToList();
            }
        }

        public string GetFileDirectory(string _FullFileNameWithPath)
        {
            if (File.Exists(_FullFileNameWithPath))
            {
                return Path.GetDirectoryName(_FullFileNameWithPath);
            }
            else { return string.Empty; }
        }

        public string GetFileName(string _FullFileNameWithPath)
        {
            if (File.Exists(_FullFileNameWithPath))
            {
                return Path.GetFileNameWithoutExtension(_FullFileNameWithPath);
            }
            else { return string.Empty; }
        }

        public string GetFileExtension(string _FullFileNameWithPath)
        {
            if (File.Exists(_FullFileNameWithPath))
            {
                return Path.GetExtension(_FullFileNameWithPath);
            }
            else { return string.Empty; }
        }

        public FileStream GetFileStream(string _FullFileNameWithPath)
        {
            FileStream fileStream = null;
            try
            {
                if (File.Exists(_FullFileNameWithPath))
                {
                    using (FileStream _fileStream = new FileStream(_FullFileNameWithPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                    {
                        fileStream = _fileStream;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFileStream");
                fileStream = null;
            }
            return fileStream;
        }

        public bool CreateFileFromFileStream(string _FullFileNameWithPath, FileStream _FileStream)
        {
            bool _return = false;
            try
            {
                using (FileStream fs = new FileStream(_FullFileNameWithPath, FileMode.Create))
                {
                    fs.Close();
                    _return = true;
                }
            }
            catch (Exception ex)
            {
                _return = false;
                Logger.Error(ex, "GetFileStream");
            }
            return _return;
        }

        public bool CreateFileFromFileStream(string _FullFileNameWithPath, byte[] _FileStream)
        {
            bool _return = false;
            try
            {
                using (FileStream fs = new FileStream(_FullFileNameWithPath, FileMode.Create))
                {
                    fs.Write(_FileStream, 0, _FileStream.Length);
                    fs.Close();
                    _return = true;
                }
            }
            catch (Exception ex)
            {
                _return = false;
                Logger.Error(ex, "GetFileStream");
            }
            return _return;
        }

      
        #endregion


        #region FileIO Private Methods

        private bool CopyStream(Stream _InStream, Stream _OutStream)
        {
            bool val = false;
            try
            {
                var DataQueue = new ConcurrentQueue<byte[]>();
                using (AutoResetEvent DataReady = new AutoResetEvent(false))
                {
                    using (AutoResetEvent DataProcessed = new AutoResetEvent(false))
                    {
                        var ReadDataTask = Task.Factory.StartNew(() =>
                        {
                            while (true)
                            {
                                var Data = new byte[BufferSize];
                                var BytesRead = _InStream.Read(Data, 0, Data.Length);
                                if (BytesRead != BufferSize)
                                    Data = SubArray(Data, 0, BytesRead);
                                DataQueue.Enqueue(Data);
                                DataReady.Set();
                                if (BytesRead != BufferSize)
                                    break;
                                DataProcessed.WaitOne();
                            }
                        });
                        var ProcessDataTask = Task.Factory.StartNew(() =>
                        {
                            byte[] Data;
                            do
                            {
                                DataReady.WaitOne();
                                DataQueue.TryDequeue(out Data);
                                DataProcessed.Set();
                                _OutStream.Write(Data, 0, Data.Length);
                                if (Data.Length != BufferSize)
                                    break;
                            } while (Data.Length == BufferSize);
                        });
                        ReadDataTask.Wait();
                        ProcessDataTask.Wait();
                    }
                }
                val = true;
            }
            catch
            {
                throw;
            }

            return val;
        }

        private T[] SubArray<T>(T[] _Data, int _Index, int _Length)
        {
            T[] result = new T[_Length];
            Array.Copy(_Data, _Index, result, 0, _Length);
            return result;
        }

        #endregion

    }
}

