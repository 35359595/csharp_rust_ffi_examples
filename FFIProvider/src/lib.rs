extern crate libc;

use libc::c_char;
use std::ffi::CString;

#[no_mangle]
pub extern fn double_num(n: isize) -> isize {
    n * 2
}

#[no_mangle]
pub extern fn get_string() -> *mut c_char {
    CString::new("Hello").unwrap().into_raw()
}

#[no_mangle]
pub extern fn to_lower(s: *mut c_char) -> *mut c_char {
    unsafe {
        CString::new(CString::from_raw(s).to_str().unwrap().to_lowercase())
            .unwrap()
            .into_raw()
    }
}

#[no_mangle]
pub extern fn string_free(s: *mut c_char) {
    unsafe {
        if s.is_null() {
            return;
        }
        CString::from_raw(s);
    };
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn double_test() {
        assert_eq!(double_num(2), 4);
    }

    #[test]
    fn get_string_test() {
        unsafe {
            assert_eq!(CString::from_raw(get_string()).to_str().unwrap(), "Hello");
        }
    }

    #[test]
    fn to_lower_test() {
        unsafe {
            let result = CString::from_raw(to_lower(CString::new("BIG LETTERS stRInG").unwrap().into_raw()));
            assert_eq!(result,
                CString::new("big letters string").unwrap());
            string_free(result.into_raw());
        }
    }
}
