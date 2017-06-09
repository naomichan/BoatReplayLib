﻿using System;

namespace BoatReplayLib {
    public class FieldNotFoundException : Exception {
        private Type t;
        public Type T => t;

        private string field;
        public string Field => field;

        public FieldNotFoundException(string field, Type t) : base($"Field {field} not found in type {t.FullName}.") {
            this.field = field;
            this.t = t;
        }
    }
}